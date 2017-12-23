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

        /// <summary>
        /// Creates instance of <see cref="MockRestartManagerService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of <see cref="IRestartManagerService"/> service to mock.</typeparam>
        /// <param name="container">The parent <see cref="MockContainer"/> to which mocks are added.</param>
        /// <param name="sessionId">Optional session ID to return. The default is 0.</param>
        /// <param name="sessionKey">Optional session key to return. The default is "123abc".</param>
        /// <param name="error">Optional error to return. The default is 0 (no error).</param>
        /// <returns>A <see cref="MockRestartManagerService"/>.</returns>
        public static MockRestartManagerService Create<TService>(MockContainer container, int sessionId = MockRestartManagerService.DefaultSessionId, string sessionKey = MockRestartManagerService.DefaultSessionKey, int error = NativeMethods.ERROR_SUCCESS)
            where TService : IRestartManagerService
        {
            var mock = new MockRestartManagerService(container)
                .StartSession(sessionId, sessionKey, error);

            if (error == NativeMethods.ERROR_SUCCESS)
            {
                mock.EndSession(sessionId);
            }

            return mock;
        }
    }
}
