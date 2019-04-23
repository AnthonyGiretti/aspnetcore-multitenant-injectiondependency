using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Plugins.Tools;
using Plugins.Tools.Extensions;

namespace Plugins.DependencyInjection.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers in DI a delegate that finds in <see cref="IContainer"/> the correct instance for a given Interface and tenant name
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="services"><see cref="IServiceCollection"/> to register plugin delegates</param>
        /// <param name="getContainer"><see cref="Func{T}"/> that returns <see cref="IContainer"/></param>
        /// <param name="pluggableItems">Collection of <see cref="PluggableItem"/> to be filtered by a tenant key and Interface</param>
        public static void RegisterPluginDelegate<TInterface>(this IServiceCollection services, Func<IContainer> getContainer, IEnumerable<PluggableItem> pluggableItems)
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

                return (TInterface)serviceProvider.GetService(items.First().Interface);

            });
        }
    }
}