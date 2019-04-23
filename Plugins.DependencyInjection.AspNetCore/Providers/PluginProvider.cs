using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Plugins.Abstractions.Providers;
using System;

namespace Plugins.DependencyInjection.AspNetCore.Providers
{
    /// <inheritdoc/>
    public class PluginProvider<TInterface> : IPluginProvider<TInterface>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PluginProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public TInterface GetInstance(string key)
        {
            var serviceFunc = this.GetServiceFunction();
            return serviceFunc(key);
        }

        /// <summary>
        /// Gets the delegate for a given pair of key / interface
        /// </summary>
        /// <returns><see cref="Func{T1, TInterface}"/></returns>
        private Func<string, TInterface> GetServiceFunction()
        {
            return (Func<string, TInterface>)_httpContextAccessor.HttpContext.RequestServices.GetRequiredService(typeof(Func<string, TInterface>));
        }
    }
}