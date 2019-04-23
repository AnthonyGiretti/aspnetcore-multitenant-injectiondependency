using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.Tools.Extensions
{
    public static class PluggableItemExtensions
    {
        /// <summary>
        /// Filters <see cref="PluggableItem"/> collection by interface type
        /// </summary>
        /// <param name="pluggableItems">Collection of <see cref="PluggableItem"/> to be filtered</param>
        /// <param name="interfaceType">Generic type for a given Interface</param>
        /// <returns>Collection of <see cref="PluggableItem"/></returns>
        public static IEnumerable<PluggableItem> FilterByInterface(this IEnumerable<PluggableItem> pluggableItems, Type interfaceType)
        {
            return pluggableItems.Where(i => i.Interface == interfaceType);
        }

        /// <summary>
        /// Filters <see cref="PluggableItem"/> collection by interface type
        /// </summary>
        /// <param name="pluggableItems">Collection of <see cref="PluggableItem"/> to be filtered</param>
        /// <param name="tenant">Tenant name</param>
        /// <returns>Collection of <see cref="PluggableItem"/></returns>
        public static IEnumerable<PluggableItem> FilterByTenant(this IEnumerable<PluggableItem> pluggableItems, string tenant)
        {
            return pluggableItems.Where(t => t.Tenant == tenant);
        }
    }
}