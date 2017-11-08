// <copyright file="Extensions.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Gets whether the <paramref name="source"/> is null or an empty collection.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="source">An enumeration of type <typeparamref name="T"/>.</param>
        /// <returns>True if <paramref name="source"/> is null or an empty collection.</returns>
        internal static bool NullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

        /// <summary>
        /// Gets a service interface of type <typeparamref name="T"/> if <paramref name="service"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of service interface to get.</typeparam>
        /// <param name="services">A service provider.</param>
        /// <param name="service">The reference to a service to initialize. This reference value will be set on return.</param>
        /// <param name="throwIfNotDefined">If true, a <see cref="NotImplementedException"/> will be thrown if the service interface is not defined. The default is false.</param>
        /// <returns>A implementation of the service interface; otherwise, null if <paramref name="throwIfNotDefined"/> is false (default).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
        /// <exception cref="NotImplementedException">No implementation of the service interface of type <typeparamref name="T"/> was found.</exception>
        internal static T GetService<T>(this IServiceProvider services, ref T service, bool throwIfNotDefined = false)
            where T : class
        {
            Validate.NotNull(services, nameof(services));

            if (service == null)
            {
                service = services.GetService<T>(throwIfNotDefined: throwIfNotDefined);
            }

            return service;
        }
    }
}
