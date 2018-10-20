using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abstractions;
using Plugins.NATS;
using Plugins.Host.AspNetCore;


namespace RacingConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NatsConfig>(options => Configuration.GetSection("Nats").Bind(options));

            // Service can both handle command and push it to a controller
            services.AddAbstractions();
            
            services.AddTransient<ICommandHandler<CalculateSumCommand>, CalculateSumCommandHandler>();
            services.AddTransient<IInformativeHandler<Completed<CalculateSumCommand, int>>, CalculateSumResultHandler>();
            services.AddSubscriptions();
            services.AddNats();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSubscriptions((subscribeTo) => 
                subscribeTo
                .CommandOf<CalculateSumCommand>()
                .ResultOf<Completed<CalculateSumCommand, int>>()
            );
            
            app.UseMvc();
        }
    }
}
