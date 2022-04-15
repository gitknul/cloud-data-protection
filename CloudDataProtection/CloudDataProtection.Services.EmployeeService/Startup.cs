using AutoMapper;
using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Jwt.Options;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.EmployeeService.Business;
using CloudDataProtection.Services.EmployeeService.Controllers.Dto.Input;
using CloudDataProtection.Services.EmployeeService.Controllers.Dto.Output;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CloudDataProtection.Services.EmployeeService
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CloudDataProtection EmployeeService", Version = "v1"});
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
            
            services.ConfigureAuthentication(Configuration);
            services.ConfigureAuthorization();

            services.Configure<RabbitMqConfiguration>(options => Configuration.GetSection("RabbitMq").Bind(options));
            services.Configure<JwtSecretOptions>(options => Configuration.GetSection("Jwt").Bind(options));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<EmployeeBusinessLogic>();
            
            services.AddAutoMapper(ConfigureMapper);
            
            services.AddEncryptedDbContext<IEmployeeDbContext, EmployeeDbContext>(Configuration, 
                o => o.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudDataProtection EmployeeService"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<Employee, EmployeeOutput>();

            config.CreateMap<CreateEmployeeInput, Employee>()
                .ForMember(d => d.FirstName, o => o.MapFrom(d => d.FirstName.Trim()))
                .ForMember(d => d.LastName, o => o.MapFrom(d => d.LastName.Trim()))
                .ForMember(d => d.ContactEmailAddress, o => o.MapFrom(d => d.ContactEmailAddress.Trim()))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(d => d.PhoneNumber.Trim()))
                .ForMember(d => d.UserEmailAddress, o => o.MapFrom(d => d.ContactEmailAddress.Trim()));
        }
    }
}