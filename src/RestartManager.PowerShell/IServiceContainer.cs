// <copyright file="IServiceContainer.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Modifiable container for service interface implementations.
    /// </summary>
    internal interface IServiceContainer : IServiceProvider
    {
        /// <summary>
        /// Adds a service interface implementation to the container.
        /// </summary>
        /// <typeparam name="T">The type of sevice interface to get.</typeparam>
        /// <param name="service">The implementation of a service interface to add.</param>
        /// <param name="override">Whether to override an existing service. If false, an <see cref="ArgumentException"/> is thrown.</param>
        /// <exception cref="ArgumentException"><paramref name="override"/> is true and the service interface is already defined.</exception>
        void AddService<T>(T service, bool @override = false)
            where T : class;
    }
}
