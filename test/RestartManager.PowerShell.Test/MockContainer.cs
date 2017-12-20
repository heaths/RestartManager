// <copyright file="MockContainer.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Linq.Expressions;
    using Moq;

    /// <summary>
    /// A mock <see cref="IServiceProvider"/>.
    /// </summary>
    internal class MockContainer : IServiceProvider
    {
        private readonly MockRepository repo;
        private readonly Mock<IServiceProvider> mocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockContainer"/> class.
        /// </summary>
        /// <param name="behavior">The <see cref="MockBehavior"/> to use. The default is <see cref="MockBehavior.Default"/>.</param>
        public MockContainer(MockBehavior behavior = MockBehavior.Default)
        {
            repo = new MockRepository(behavior);
            mocks = repo.Create<IServiceProvider>();
        }

        /// <summary>
        /// Gets the underlying <see cref="IServiceProvider"/>.
        /// </summary>
        internal IServiceProvider Object => mocks.Object;

        /// <summary>
        /// Gets the <see cref="MockRepository"/> used to create mocks.
        /// </summary>
        internal MockRepository Repository => repo;

        /// <inheritdoc/>
        public TService GetService<TService>(bool throwIfNotDefined = false)
            where TService : class
        {
            return Object.GetService<TService>(throwIfNotDefined);
        }

        /// <summary>
        /// Pushes a new <see cref="MockService{TService}"/> scope.
        /// </summary>
        /// <typeparam name="TService">The type of service to mock.</typeparam>
        /// <typeparam name="TMock">The <see cref="MockService{TService}"/> implementing the <typeparamref name="TService"/> to push.</typeparam>
        /// <returns>A <see cref="MockService{TService}"/> implementing <typeparamref name="TService"/>.</returns>
        internal TMock Push<TService, TMock>()
            where TService : class
            where TMock : MockService<TService>, TService
        {
            var mock = MockServiceFactory.Create<TService, TMock>(this);
            AddService(mock.Object);

            return mock;
        }

        /// <summary>
        /// Gets a <see cref="Mock{T}"/> for the <typeparamref name="TService"/> service interface.
        /// </summary>
        /// <typeparam name="TService">The type of service being mocked.</typeparam>
        /// <returns>A <see cref="Mock{T}"/> for the <typeparamref name="TService"/> service interface.</returns>
        /// <exception cref="NotImplementedException"><typeparamref name="TService"/> is not mocked.</exception>
        internal Mock<TService> Get<TService>()
            where TService : class
        {
            var service = Object.GetService<TService>();
            if (service == null)
            {
                throw new NotImplementedException();
            }

            var mock = Mock.Get(service);
            return mock;
        }

        /// <summary>
        /// Verifies all verifiable setups in the <see cref="Repository"/>.
        /// </summary>
        /// <remarks>
        /// All services should make all explicit setups verifiable.
        /// </remarks>
        internal void Verify()
        {
            repo.Verify();
        }

        /// <summary>
        /// Verifies the <paramref name="expression"/> on the given <typeparamref name="TService"/> service interface.
        /// </summary>
        /// <typeparam name="TService">The service interface that is mocked.</typeparam>
        /// <param name="expression">The expression to verify.</param>
        /// <param name="times">The number of times the expression should be called. The default is <see cref="Times.AtLeastOnce"/>.</param>
        /// <exception cref="NotImplementedException"><typeparamref name="TService"/> is not mocked.</exception>
        internal void Verify<TService>(Expression<Action<TService>> expression, Func<Times> times = null)
            where TService : class
        {
            var mock = Get<TService>();
            mock.Verify(expression, times ?? Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies the <paramref name="expression"/> on the given <typeparamref name="TService"/> service interface.
        /// </summary>
        /// <typeparam name="TService">The service interface that is mocked.</typeparam>
        /// <param name="expression">The expression to verify.</param>
        /// <param name="times">The number of times the expression should be called.</param>
        /// <exception cref="NotImplementedException"><typeparamref name="TService"/> is not mocked.</exception>
        internal void Verify<TService>(Expression<Action<TService>> expression, Times times)
            where TService : class
        {
            var mock = Get<TService>();
            mock.Verify(expression, times);
        }

        /// <summary>
        /// Verifies the <paramref name="expression"/> on the given <typeparamref name="TService"/> service interface.
        /// </summary>
        /// <typeparam name="TService">The service interface that is mocked.</typeparam>
        /// <typeparam name="TResult">The type of result that is returned by the <paramref name="expression"/>.</typeparam>
        /// <param name="expression">The expression to verify.</param>
        /// <param name="times">The number of times the expression should be called. The default is <see cref="Times.AtLeastOnce"/>.</param>
        /// <exception cref="NotImplementedException"><typeparamref name="TService"/> is not mocked.</exception>
        internal void Verify<TService, TResult>(Expression<Func<TService, TResult>> expression, Func<Times> times = null)
            where TService : class
        {
            var mock = Get<TService>();
            mock.Verify(expression, times ?? Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies the <paramref name="expression"/> on the given <typeparamref name="TService"/> service interface.
        /// </summary>
        /// <typeparam name="TService">The service interface that is mocked.</typeparam>
        /// <typeparam name="TResult">The type of result that is returned by the <paramref name="expression"/>.</typeparam>
        /// <param name="expression">The expression to verify.</param>
        /// <param name="times">The number of times the expression should be called.</param>
        /// <exception cref="NotImplementedException"><typeparamref name="TService"/> is not mocked.</exception>
        internal void Verify<TService, TResult>(Expression<Func<TService, TResult>> expression, Times times)
            where TService : class
        {
            var mock = Get<TService>();
            mock.Verify(expression, times);
        }

        private void AddService<TService>(TService service)
            where TService : class
        {
            mocks.Setup(x => x.GetService<TService>(It.IsAny<bool>())).Returns(service);
        }
    }
}
