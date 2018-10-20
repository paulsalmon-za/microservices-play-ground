using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Abstractions;

namespace Plugins.Host.AspNetCore
{
    public static class SubscriptionRegistrationBuilderExtensions
    {
        public static IServiceCollection AddSubscriptions(this IServiceCollection services)
        {   
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<SubscriptionRegistration>();

            return services;
        }

        public static IApplicationBuilder UseSubscriptions(this IApplicationBuilder app, Func<SubscriptionRegistrationBuilder,SubscriptionRegistrationBuilder> builder)
        {   
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var resolver = app.ApplicationServices.GetService<IInformationNameResolver>();
            var registrations = app.ApplicationServices.GetService<SubscriptionRegistration>();
            builder(new SubscriptionRegistrationBuilder(resolver, registrations));
            
            return app;
        }

    }
}