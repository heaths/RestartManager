// <copyright file="Extensions.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
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
        /// <param name="services">Optional service provider.</param>
        /// <param name="service">The reference to a service to initialize. This reference value will be set on return.</param>
        /// <param name="factory">Creates a new instance of <typeparamref name="T"/> if <paramref name="services"/> or <paramref name="service"/> is null.</param>
        /// <returns>A implementation of the service interface.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
        internal static T GetService<T>(this IServiceProvider services, ref T service, Func<T> factory)
            where T : class
        {
            Validate.NotNull(factory, nameof(factory));

            if (service == null)
            {
                service = services?.GetService<T>() ?? factory();
            }

            return service;
        }
    }
}
