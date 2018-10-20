using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Abstractions;

namespace Plugins.NATS
{
    public static class NatsBuilderExtensions
    {
        public static IServiceCollection AddNats(this IServiceCollection services)
        {   
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            services.AddTransient(typeof(ICommandDispatcher<>),typeof(NatsCommandDispatcher<>));
            services.AddTransient(typeof(IInformativeDispatcher<>), typeof(NatsPubSubDispatcher<>));
            services.AddHostedService<NatsHostedService>();
            services.AddTransient<NatsConfig>(ResolveConfig);
            return services;
        }

        private static NatsConfig ResolveConfig(IServiceProvider provider)
        {
            var options = provider.GetService<IOptions<NatsConfig>>();
            if (options == null || options.Value == null)
            {
                return new NatsConfig() { Host = "localhost", Port = 4222 };
            }
            else {
                return options.Value;
            }
        }

        public static IApplicationBuilder UserNats(this IApplicationBuilder app)
        {   
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = app.ApplicationServices.GetService<IOptions<NatsConfig>>();
            NatsConfig config = null;
            if (options == null || options.Value == null)
            {
                config = new NatsConfig() { Host = "localhost", Port = 4222 };
            }
            else {
                config = options.Value;
            }

        

            return app;
        }

    }
}