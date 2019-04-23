using Plugins.Abstractions.Constants;
using Plugins.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugins.Tools.Extensions
{
    /// <summary>
    /// Provides extensions method on <see cref="Assembly"/>
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Discovers <see cref="Type"/> for a given assemblyName prefix and a given <see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="TAttribute">Generic type for a given <see cref="Attribute"/></typeparam>
        /// <param name="assemblies">Array of <see cref="Assembly"/> to be fetched</param>
        /// <param name="assemblyPrefix">Assembly prefix to be fetched</param>
        /// <returns>Collection of <see cref="PluggableItem"/></returns>
        public static IEnumerable<PluggableItem> FetchPlugins<TAttribute>(this Assembly[] assemblies, string assemblyPrefix) where TAttribute : Attribute
        {
 
            var lifeTimePropertyName = "LifeTime";

            List<PluggableItem> pluggableTypesToRegister = new List<PluggableItem>();

            var fetchedAssemblies = AppDomain.CurrentDomain
                                             .GetAssemblies()
                                             .FilterByName(assemblyPrefix)
                                             .ToList();

            fetchedAssemblies.ForEach(x =>
            {
                var tenantName = x.GetTenant();

                // all concrete classes to register
                var allTypes = x.GetConcreteTypes<TAttribute>().ToList();

                // all interfaces
                allTypes.ForEach(t =>
                {
                    var @interface = t.GetInterfaces().FirstOrDefault();

                    if (@interface != null)
                    {
                        var lifeTime = t.GetAttributeValue<TAttribute>(lifeTimePropertyName);

                        pluggableTypesToRegister.Add(new PluggableItem
                        {
                            Tenant = tenantName,
                            Interface = @interface,
                            Service = t,
                            LifeTime = (PluginLifeTime?)lifeTime ?? PluginConstants.DefaultLifeTime
                        });
                    }
                });
            });
            return pluggableTypesToRegister;
        }

        /// <summary>
        /// Gets assemblies that match an assemblyName
        /// </summary>
        /// <param name="assemblies">Array of <see cref="Assembly"/> to be filtered</param>
        /// <param name="assemblyName">Assembly name filter</param>
        /// <returns><see cref="Assembly"/> collection</returns>
        public static IEnumerable<Assembly> FilterByName(this Assembly[] assemblies, string assemblyName)
        {
            return assemblies.Where(x => x.GetName()
                                          .Name
                                          .StartsWith(assemblyName));
        }

        /// <summary>
        /// Extract tenant name from assembly name
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> to extract tenant name</param>
        /// <returns><see cref="string"/></returns>
        public static string GetTenant(this Assembly assembly)
        {
            var assemblyName = assembly.FullName
                                       .Split(',')[0]
                                       .Split('.');
            var lastIndex = assemblyName.Length - 1;
            return assemblyName[lastIndex];
        }

        /// <summary>
        /// Gets concrete type that have TAttribute in Attribute 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="assembly"><see cref="Assembly"/> to extract concrete <see cref="Type"/></param>
        /// <returns><see cref="Type"/> collection</returns>
        public static IEnumerable<Type> GetConcreteTypes<TAttribute>(this Assembly assembly) where TAttribute : Attribute
        {
            return assembly.GetTypes()
                           .Where(t => t.GetCustomAttributes()
                                        .Any(a => a.GetType() == typeof(TAttribute)));
        }
    }
}
