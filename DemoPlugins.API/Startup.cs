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
using Plugins.Services.Tenant1;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace DemoPlugins.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // Fetch plugin types
            var pluggableTypesToRegister = LoadAssemblies()
                                           .ToArray()
                                           .FetchPlugins<PluggableAttribute>("Plugins.Services.Tenant")
                                           .ToList();

            // Instance provider
            services.RegisterPluginProvider();

            // Register pluggable services
            services.RegisterPlugins(pluggableTypesToRegister);

            // Register plugin delegate
            services.RegisterPluginDelegate<IHelloWorldService>(pluggableTypesToRegister);
        }

        private IEnumerable<Assembly> LoadAssemblies()
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directoryName = Path.GetDirectoryName(location);

            DirectoryInfo directory = new DirectoryInfo(directoryName);
            FileInfo[] files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                // Load the file into the application domain.
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
                yield return assembly;
            }

            yield break;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
