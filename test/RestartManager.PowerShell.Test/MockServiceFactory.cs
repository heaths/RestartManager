// <copyright file="MockServiceFactory.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// A factory to create instances of <see cref="MockService{TService}"/>.
    /// </summary>
    internal static class MockServiceFactory
    {
        /// <summary>
        /// Creates instance of <see cref="MockService{TService}"/> for the given <typeparamref name="TMock"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to mock.</typeparam>
        /// <typeparam name="TMock">The <see cref="MockService{TService}"/> implementing the <typeparamref name="TService"/> to push.</typeparam>
        /// <param name="container">The parent <see cref="MockContainer"/> to which mocks are added.</param>
        /// <returns>A <see cref="MockService{TService}"/> implementing <typeparamref name="TService"/>.</returns>
        public static TMock Create<TService, TMock>(MockContainer container)
            where TService : class
            where TMock : MockService<TService>, TService
        {
            return Activator.CreateInstance(typeof(TMock), container) as TMock;
        }
    }
}
