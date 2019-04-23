using Microsoft.Extensions.DependencyInjection;
using Plugins.Tools;
using Plugins.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Plugins.Abstractions.Enums;
using Plugins.Abstractions.Providers;
using Plugins.DependencyInjection.AspNetCore.Providers;

namespace Plugins.DependencyInjection.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers in DI a delegate that finds in <see cref="IServiceCollection"/> the correct instance for a given interface and tenant name
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="services"><see cref="IServiceCollection"/> to register plugin delegates</param>
        /// <param name="pluggableItems">Collection of <see cref="PluggableItem"/> to be filtered by a tenant key and Interface</param>
        public static void RegisterPluginDelegate<TInterface>(this IServiceCollection services, IEnumerable<PluggableItem> pluggableItems)
        {
            // Prefilter by interface at startup
            var filteredItems = pluggableItems.FilterByInterface(typeof(TInterface));
            
            services.AddScoped<Func<string, TInterface>>(serviceProvider => (tenant) =>
            {
                var items = filteredItems.FilterByTenant(tenant);

                if (!items.Any())
                {
                    throw new KeyNotFoundException($"No instance found for the tenant '{tenant}'.");
                }

                if (items.Count() > 1)
                {
                    throw new AmbiguousMatchException($"Several instances have been found for the tenant '{tenant}'.");
                }

                return (TInterface)serviceProvider.GetRequiredService(items.First().Interface);

            });
        }

        /// <summary>
        /// Registers in DI a collection of concrete types
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to register plugins</param>
        /// <param name="pluggableItems">Collection of <see cref="PluggableItem"/> to be registered</param>
        public static void RegisterPlugins(this IServiceCollection services, IEnumerable<PluggableItem> pluggableItems)
        {
            foreach (var pluggableItem in pluggableItems)
            {
                Action<Type> registrationFunc;
                switch (pluggableItem.LifeTime)
                {
                    case PluginLifeTime.Scoped:
                        registrationFunc = (type) => services.AddScoped(type);
                        break;

                    case PluginLifeTime.Singleton:
                        registrationFunc = (type) => services.AddSingleton(type);
                        break;

                    case PluginLifeTime.Transient:
                        registrationFunc = (type) => services.AddTransient(type);
                        break;

                    default:
                        registrationFunc = (type) => services.AddScoped(type);
                        break;
                }
                registrationFunc(pluggableItem.Service);
            }
        }

        /// <summary>
        /// Registers in DI <see cref="IPluginProvider{TInterface}"/> and its concrete instance <see cref="PluginProvider{TInterface}"/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to register <see cref="PluginProvider{T}"/> and <see cref="IPluginProvider{T}"/></param>
        public static void RegisterPluginProvider(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IPluginProvider<>), typeof(PluginProvider<>));
        }
    }
}