using System.Reflection;
using EmployeeManagement.Database;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Database.Repositories;
using EmployeeManagement.Shared.Constants;
using EmployeeManagement.Shared.Services;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;

namespace EmployeeManagement.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IEmployeeRepository), typeof(EmployeeRepository));
            services.AddTransient(typeof(ICountryRepository), typeof(CountryRepository));
            services.AddTransient(typeof(IStateRepository), typeof(StateRepository));
            services.AddTransient(typeof(ICityRepository), typeof(CityRepository));

            return services;
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]{
                Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(IBaseService))
            };

            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                 .Where(c => c.Name.EndsWith("Service"))
                 .AsPublicImplementedInterfaces();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(Constants.SQL_CONNECTION_STRING_KEY));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection(Constants.APP_SETTINGS_KEY).GetValue<string>(Constants.CLIENT_APP_URL_KEY);

            string[]? urls = origins?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            services.AddCors(c =>
            {
                c.AddPolicy(Constants.CQRS_KEY, builder =>
                {
                    builder
                    .WithOrigins(urls!)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
    }
}
