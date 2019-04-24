using DemoMultiTenancy.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plugins.Abstractions.Attributes;
using Plugins.DependencyInjection.AspNetCore.Extensions;
using Plugins.Tools.Extensions;
using System;
using System.Linq;

namespace DemoMultiTenancy.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Fetch plugin types
            var pluggableTypesToRegister = AppDomain.CurrentDomain
                .GetAssemblies()
                .FetchPlugins<PluggableAttribute>("Tenants.Services")
                .ToList();

            // Instance provider
            services.RegisterPluginProvider();

            // Register pluggable services
            services.RegisterPlugins(pluggableTypesToRegister);

            // Register plugin delegate
            services.RegisterPluginDelegate<IHelloWorldService>(pluggableTypesToRegister);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
