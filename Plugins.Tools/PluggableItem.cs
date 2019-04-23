using Plugins.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Tools
{
    public class PluggableItem
    {
        /// <summary>
        /// Gets or sets Tenant
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// Gets or sets Interface
        /// </summary>
        public Type Interface { get; set; }

        /// <summary>
        /// Gets or sets Service
        /// </summary>
        public Type Service { get; set; }

        /// <summary>
        /// Gets or sets LifeTime
        /// </summary>
        public PluginLifeTime LifeTime { get; set; }
    }
}