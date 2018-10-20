using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Abstractions;

namespace Plugins.Host.AspNetCore
{
    public static class AbstractionsBuilderExtensions
    {
        public static IServiceCollection AddAbstractions(this IServiceCollection services)
        {   
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IDynamicConverter, JObjectDynamicConverter>();
            services.AddTransient<ICorrelationService, SequentialCorrelationService>();
            services.AddTransient<IDependencyResolver, HostDependencyResolver>();
            services.AddTransient<IInformationNameResolver, SimpleInformationNameResolver>();
            services.AddTransient<IDispatchService, DispatchService>();
            services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));

            return services;
        }

    }
}