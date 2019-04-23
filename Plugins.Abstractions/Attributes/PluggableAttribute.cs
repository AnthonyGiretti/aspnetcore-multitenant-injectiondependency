using System;
using System.Collections.Generic;
using System.Text;
using Plugins.Abstractions.Constants;
using Plugins.Abstractions.Enums;

namespace Plugins.Abstractions.Attributes
{
    /// <summary>
    /// Allows services to be discovered for plugins registration
    /// </summary>
    public class PluggableAttribute : Attribute
    {
        public PluginLifeTime LifeTime { get; }

        public PluggableAttribute()
        {
            this.LifeTime = PluginConstants.DefaultLifeTime;
        }

        public PluggableAttribute(PluginLifeTime lifeTime)
        {
            this.LifeTime = lifeTime;
        }
    }
}
