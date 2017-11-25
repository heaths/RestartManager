// <copyright file="ServiceContainer.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Service container.
    /// </summary>
    internal class ServiceContainer : IServiceContainer
    {
        private readonly IDictionary<Type, object> services = new Dictionary<Type, object>();

        /// <inheritdoc/>
        public void AddService<T>(T service, bool @override = false)
            where T : class
        {
            Validate.NotNull(service, nameof(service));

            // Make sure we're adding only interfaces.
            Debug.Assert(typeof(T).IsInterface, $"Type '{typeof(T)}' is not a service interface");

            var key = typeof(T);
            if (!services.ContainsKey(key))
            {
                services.Add(key, service);
            }
            else if (@override)
            {
                services[key] = service;
            }
            else
            {
                throw new ArgumentException(Properties.Resources.Error_ServiceExists, nameof(service));
            }
        }

        /// <inheritdoc/>
        public T GetService<T>(bool throwIfNotDefined = false)
            where T : class
        {
            if (!services.TryGetValue(typeof(T), out var service) && throwIfNotDefined)
            {
                throw new NotImplementedException();
            }

            return (T)service;
        }
    }
}
