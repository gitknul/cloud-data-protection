using System;
using System.Threading;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.RabbitMq.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CloudDataProtection.Core.Messaging.RabbitMq
{
    public abstract class RabbitMqMessageListener<TModel> : BackgroundService, IMessageListener<TModel> where TModel : class
    {
        private readonly ILogger<RabbitMqMessageListener<TModel>> _logger;
        private readonly RabbitMqConfiguration _configuration;

        protected abstract string RoutingKey { get; }

        private ConnectionFactory _connectionFactory;
        private ConnectionFactory ConnectionFactory
        {
            get
            {
                if (_connectionFactory == null)
                {
                    _connectionFactory = new ConnectionFactory
                    {
                        HostName = _configuration.Hostname,
                        Port = _configuration.Port,
                        UserName = _configuration.UserName,
                        Password = _configuration.Password,
                        VirtualHost = _configuration.VirtualHost,
                        ClientProvidedName = _queue
                    };
                }
                
                return _connectionFactory;
            }
        }

        private IConnection _connection;
        private IConnection Connection => _connection ??= ConnectionFactory.CreateConnection();

        private IModel _channel;

        private readonly Type _type;

        private readonly string _queue;

        protected RabbitMqMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<RabbitMqMessageListener<TModel>> logger)
        {
            _logger = logger;
            _configuration = options.Value;
            _type = GetType();
            _queue = GetQueueName();
            
            Init();
        }
        
        public abstract Task HandleMessage(TModel model);
        
        protected sealed override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (_ ,args) => await HandleMessage(args);

            _channel.BasicConsume(_queue, false, consumer);

            return Task.CompletedTask;
        }

        public sealed override Task StopAsync(CancellationToken cancellationToken)
        {
            _connection?.Close();
            
            return base.StopAsync(cancellationToken);
        }

        private async Task HandleMessage(BasicDeliverEventArgs args)
        {
            _logger.LogInformation("Received message with delivery tag {DeliveryTag}", args.DeliveryTag);

            try
            {
                TModel model = args.GetModel<TModel>();
                
                await HandleMessage(model);
                
                _channel.BasicAck(args.DeliveryTag, false);
            }
            catch (JsonReaderException e)
            {
                _logger.LogWarning(e, "Could not deserialize message with delivery tag {DeliveryTag}", args.DeliveryTag);

                _channel.BasicNack(args.DeliveryTag, false, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while handling message with delivery tag {DeliveryTag}", args.DeliveryTag);
                
                _channel.BasicNack(args.DeliveryTag, false, true);
            }
        }

        private string GetQueueName()
        {
            return _type.Assembly.GetName().Name + "__" + _type.Name;
        }

        private void Init()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(_configuration.RetryCount, GetRetryDelay, OnInitRetry)
                .Execute(DoInit);
        }


        private void OnInitRetry(Exception exception, TimeSpan timeSpan, int attempt, Context context)
        {
            _logger.LogWarning(exception, "An error occurred during initialization");
            _logger.LogWarning("Retrying initialization. Waiting {Ms}ms. Attempt nr. {Attempt} / {RetryCount}", timeSpan.TotalMilliseconds, attempt, _configuration.RetryCount);
        }
        
        private TimeSpan GetRetryDelay(int attempt)
        {
            return TimeSpan.FromMilliseconds(attempt * _configuration.RetryDelayMs);
        }

        private void DoInit()
        {
            _channel = Connection.CreateModel();
            _channel.ExchangeDeclare(_configuration.Exchange, ExchangeType.Direct, true);

            _channel.QueueDeclare(_queue, exclusive: false, durable: true, autoDelete: false);
            _channel.QueueBind(_queue, _configuration.Exchange, RoutingKey);
        }
    }
}

