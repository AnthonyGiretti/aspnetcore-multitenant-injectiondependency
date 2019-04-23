using System;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Abstractions.Providers
{
    /// <summary>
    /// Interface that provides method contracts to find concrete instance for a given Interface and key
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public interface IPluginProvider<out TInterface>
    {
        /// <summary>
        /// Get instance by key
        /// </summary>
        /// <param name="key">Key of the instance ex: tenantName</param>
        /// <returns><see cref="TInterface"/></returns>
        TInterface GetInstance(string key);
    }
}
