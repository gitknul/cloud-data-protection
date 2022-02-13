using AutoMapper;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Controllers.Dto.Output;
using CloudDataProtection.Services.Subscription.Data.Context;
using CloudDataProtection.Services.Subscription.Data.Repository;
using CloudDataProtection.Services.Subscription.Entities;
using CloudDataProtection.Services.Subscription.Messaging.Listener;
using CloudDataProtection.Services.Subscription.Messaging.Publisher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CloudDataProtection.Services.Subscription
{
    public class Startup
    {
        private static readonly string CorsPolicy = "cors-policy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection BackupConfigurationService", Version = "v1"});
            });

            services.AddScoped<IBackupConfigurationRepository, BackupConfigurationRepository>();
            services.AddScoped<IBackupSchemeRepository, BackupSchemeRepository>();

            services.AddLazy<BackupSchemeBusinessLogic>();
            services.AddLazy<BackupConfigurationBusinessLogic>();
            
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            
            services.AddLazy<IMessagePublisher<BackupConfigurationEnteredMessage>, BackupConfigurationEnteredMessagePublisher>();
            services.AddLazy<IMessagePublisher<UserDataDeletedMessage>, UserDataDeletedMessagePublisher>();

            services.AddHostedService<UserDeletedMessageListener>();

            services.AddEncryptedDbContext<IBackupConfigurationDbContext, BackupConfigurationDbContext>(Configuration, builder =>
            {
                builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });
            
            ConfigureAuthentication(services);

            services.AddAutoMapper(ConfigureMapper);
        }

        private void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<BackupScheme, BackupSchemeOuput>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));
            
            config.CreateMap<BackupConfiguration, BackupConfigurationOutput>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));
            
            config.CreateMap<BackupConfiguration, CreateBackupConfigurationResult>()
                .ForMember(p => p.Hour, 
                    options => options.MapFrom(s => s.Time.Hours))
                .ForMember(p => p.Minute, 
                    options => options.MapFrom(s => s.Time.Minutes));

            config.CreateMap<BackupScheme, BackupConfiguration>()
                .ForMember(p => p.Id,
                    options => options.Ignore())
                .ForMember(p => p.CreatedAt,
                    options => options.Ignore())
                .ForMember(p => p.UserId,
                    options => options.Ignore())
                .ForMember(p => p.TimeId,
                    options => options.Ignore())
                .ForMember(p => p.Time,
                    options => options.Ignore())
                .ForPath(p => p.Time.Hours,
                    options => options.MapFrom(s => s.Time.Hours))
                .ForPath(p => p.Time.Minutes,
                    options => options.MapFrom(s => s.Time.Minutes))
                .ForPath(p => p.Time.Seconds,
                    options => options.MapFrom(s => s.Time.Seconds));
        }
        
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.ConfigureAuthentication(Configuration);
            services.ConfigureAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackupConfigurationService");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}