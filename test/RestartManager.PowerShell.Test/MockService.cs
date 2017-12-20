// <copyright file="MockService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using Moq;

    /// <summary>
    /// Base class for mock services.
    /// </summary>
    /// <typeparam name="TService">The type of service interface to mock.</typeparam>
    internal abstract class MockService<TService>
        where TService : class
    {
        private readonly MockContainer container;
        private readonly Mock<TService> mock;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockService{TService}"/> class.
        /// </summary>
        /// <param name="container">The parent <see cref="MockContainer"/>.</param>
        protected MockService(MockContainer container)
        {
            this.container = container;
            mock = container.Repository.Create<TService>();
        }

        /// <summary>
        /// Gets the mock service.
        /// </summary>
        public TService Object => mock.Object;

        /// <summary>
        /// Gets the <see cref="Mock{T}"/> for the service.
        /// </summary>
        protected Mock<TService> Mock => mock;

        /// <summary>
        /// Gets the parent <see cref="MockContainer"/> services.
        /// </summary>
        protected IServiceProvider Services => container.Object;

        /// <summary>
        /// Pops the current service scope back to the parent <see cref="MockContainer"/> for fluent calls.
        /// </summary>
        /// <returns>The parent <see cref="MockContainer"/>.</returns>
        public MockContainer Pop()
        {
            return container;
        }
    }
}
