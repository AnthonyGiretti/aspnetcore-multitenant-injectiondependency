using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plugins.Tools.Extensions
{
    /// <summary>
    /// Provides extensions on <see cref="Type"/>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets attribute value for a given <see cref="Type"/> and a given <see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static object GetAttributeValue<TAttribute>(this Type type, string attributeName) where TAttribute : Attribute
        {
            return type.GetCustomAttributes()
                .Where(a => a.GetType() == typeof(TAttribute))
                .Select(a => a.GetType()
                    .GetProperty(attributeName)?
                    .GetValue(a))
                .FirstOrDefault();
        }

        /// <summary>
        /// Verify if the given property belongs to the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns><see cref="bool"/></returns>
        public static bool HasProperty(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName) != null;
        }
    }
}
