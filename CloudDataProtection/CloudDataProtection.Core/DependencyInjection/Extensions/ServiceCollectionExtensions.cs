using System;
using System.Security.Claims;
using CloudDataProtection.Core.Controllers.Data;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Jwt.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Core.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLazy<TImplementation>(this IServiceCollection services) where TImplementation : class
        {
            services.AddTransient<TImplementation>();
            services.AddTransient(provider => new Lazy<TImplementation>(() => provider.GetRequiredService<TImplementation>()));
        }
        
        public static void AddLazy<TService, TImplementation>(this IServiceCollection services) 
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddTransient(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
        }

        public static void AddEncryptedDbContext<TService, TImplementation>(this IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> optionsAction = null) 
            where TService : class 
            where TImplementation : DbContext, TService
        {
            services.Configure<AesOptions>(options => configuration.GetSection("Persistence").Bind(options));
            
            services.AddScoped<ITransformer, AesTransformer>();

            services.AddDbContext<TService, TImplementation>(optionsAction);
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecretOptions options = new JwtSecretOptions();
            
            configuration.GetSection("Jwt").Bind(options);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(options.Key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            services.AddScoped<IJwtDecoder, JwtDecoder>();
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("ClientOnly", p => 
                    p.RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, ((int)UserRole.Client).ToString())));
                x.AddPolicy("EmployeeOnly", p => 
                    p.RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, ((int)UserRole.Employee).ToString())));
                x.AddPolicy("AdminOnly", p => 
                    p.RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, ((int)UserRole.Admin).ToString())));
            });
        }
    }
}