﻿using System.Reflection;
using EmployeeManagement.Database;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Database.Repositories;
using EmployeeManagement.Infrastructure.MessageBroker;
using EmployeeManagement.Shared.Constants;
using EmployeeManagement.Shared.Services;
using Microsoft.Azure.ServiceBus;
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
            services.AddTransient(typeof(ITasksRepository), typeof(TasksRepository));

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
            var connectionString = configuration.GetConnectionString(Constants.SQL_CONNECTION_STRING_KEY);

            services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            SQLHelper.ConnectionString = connectionString!;

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

        public static void ConfigureAzureServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection(Constants.AZURE_SERVICE_BUS_CONNECTION_KEY).GetValue<string>(Constants.AZURE_SERVICE_BUS_CONNECTION_STRING);

            var queueName = configuration.GetSection(Constants.AZURE_SERVICE_BUS_CONNECTION_KEY).GetValue<string>(Constants.AZURE_SERVICE_BUS_QUEUE_NAME);  

            services.AddSingleton<IQueueClient>(x => new QueueClient(connectionString, queueName));
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
        }
    }
}
