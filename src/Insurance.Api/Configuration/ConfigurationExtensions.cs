using Insurance.BusinessRules.Services;
using Insurance.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Insurance.Api.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void AddSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Type = SecuritySchemeType.ApiKey
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme 
                        {
                            Name = "X-Api-Key",
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "X-Api-Key"
                            },
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IProductApiService, ProductApiService>();
            serviceCollection.AddTransient<ISurchageRateService, SurchageRateService>();
            serviceCollection.AddTransient<IInsuranceService, InsuranceService>();
        }
        
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = services.BuildServiceProvider();
            var environment = sp.GetService<IWebHostEnvironment>();
            var connectionString = configuration.GetConnectionString("InsuranceDb");

            if (environment.IsDevelopment())
            {
                services.AddDbContext<InsuranceContext>(opt => opt.UseInMemoryDatabase("InsuranceDb"));
            }
            else
            {
                services.AddDbContext<InsuranceContext>(opt => opt.UseSqlServer(connectionString));
            }
        }
    }
}