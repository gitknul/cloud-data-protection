using AutoMapper;
using CloudDataProtection.Core.Cryptography.Generator;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Input;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Output;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Config;
using CloudDataProtection.Services.Onboarding.Controllers.Dto.Output;
using CloudDataProtection.Services.Onboarding.Data.Context;
using CloudDataProtection.Services.Onboarding.Data.Repository;
using CloudDataProtection.Services.Onboarding.Entities;
using CloudDataProtection.Services.Onboarding.Google.Credentials;
using CloudDataProtection.Services.Onboarding.Google.Options;
using CloudDataProtection.Services.Onboarding.Messaging.Client;
using CloudDataProtection.Services.Onboarding.Messaging.Listener;
using CloudDataProtection.Services.Onboarding.Messaging.Publisher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CloudDataProtection.Services.Onboarding
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection OnboardingService", Version = "v1"});
            });
            
            services.AddScoped<IOnboardingRepository, OnboardingRepository>();
            services.AddScoped<IGoogleCredentialsRepository, GoogleCredentialsRepository>();
            services.AddScoped<IGoogleLoginTokenRepository, LoginTokenRepository>();

            services.AddLazy<IRpcClient<GetUserEmailRpcInput, GetUserEmailRpcOutput>, GetUserEmailRpcClient>();
            services.AddLazy<IMessagePublisher<GoogleAccountConnectedMessage>, GoogleAccountConnectedMessagePublisher>();
            
            services.Configure<GoogleOAuthV2Options>(options => Configuration.GetSection("Google:OAuth2").Bind(options));
            
            services.AddSingleton<ITokenGenerator, OtpGenerator>();
            services.AddSingleton<IGoogleOAuthV2CredentialsProvider, GoogleOAuthV2EnvironmentCredentialsProvider>();
            
            services.AddLazy<OnboardingBusinessLogic>();
            
            services.AddLazy<IRpcClient<GetUserEmailRpcInput, GetUserEmailRpcOutput>, GetUserEmailRpcClient>();
            services.AddLazy<IMessagePublisher<UserDataDeletedMessage>, UserDataDeletedMessagePublisher>();
            
            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            services.Configure<OnboardingOptions>(options => Configuration.GetSection("Google:Onboarding").Bind(options));

            services.AddHostedService<ClientRegisteredMessageListener>();
            services.AddHostedService<BackupConfigurationEnteredMessageListener>();
            services.AddHostedService<UserDeletedMessageListener>();

            services.AddEncryptedDbContext<IOnboardingDbContext, OnboardingDbContext>(Configuration, builder =>
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
            config.CreateMap<Entities.Onboarding, OnboardingOutput>()
                .ForMember(m => m.LoginInfo, options => options.Ignore());

            config.CreateMap<GoogleLoginInfo, GoogleLoginInfoOutput>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnboardingService");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(CorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}