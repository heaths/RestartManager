// <copyright file="IServiceProvider.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Provides service interface implementations.
    /// </summary>
    internal interface IServiceProvider
    {
        /// <summary>
        /// Gets a service interface of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of sevice interface to get.</typeparam>
        /// <param name="throwIfNotDefined">If true, a <see cref="NotImplementedException"/> will be thrown if the service interface is not defined. The default is false.</param>
        /// <returns>A implementation of the service interface; otherwise, null if <paramref name="throwIfNotDefined"/> is false (default).</returns>
        /// <exception cref="NotImplementedException">No implementation of the service interface of type <typeparamref name="T"/> was found.</exception>
        T GetService<T>(bool throwIfNotDefined = false)
            where T : class;
    }
}
