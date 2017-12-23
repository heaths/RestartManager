// <copyright file="MockVariableService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Moq;

    /// <summary>
    /// Mock <see cref="IVariableService"/>.
    /// </summary>
    internal class MockVariableService
        : MockService<IVariableService>, IVariableService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockVariableService"/> class.
        /// </summary>
        /// <param name="container">The parent <see cref="MockContainer"/>.</param>
        public MockVariableService(MockContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Mocks <see cref="IVariableService.GetValue{T}(string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of variable to get.</typeparam>
        /// <param name="name">The name of the variable to get.</param>
        /// <param name="value">The value to get.</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockVariableService GetValue<T>(string name, T value)
            where T : class
        {
            Mock.Setup(x => x.GetValue<T>(name))
                .Returns(value)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IVariableService.GetValue{T}(string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of variable to get.</typeparam>
        /// <param name="name">The name of the variable to get.</param>
        /// <param name="action">An <see cref="Action"/> to get the value.</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockVariableService GetValue<T>(string name, Func<T> action)
            where T : class
        {
            Mock.Setup(x => x.GetValue<T>(name))
                .Returns(action)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IVariableService.GetValue{T}(string)"/>.
        /// </summary>
        /// <typeparam name="T">The type of variable to get.</typeparam>
        /// <param name="name">The name of the variable to get.</param>
        /// <param name="action">A <see cref="Func{T, TResult}"/> that is passed an <see cref="RestartManager.IServiceProvider"/> to get the value.</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockVariableService GetValue<T>(string name, Func<RestartManager.IServiceProvider, T> action)
            where T : class
        {
            var value = action?.Invoke(Services);
            Mock.Setup(x => x.GetValue<T>(name))
                .Returns(value)
                .Verifiable();

            return this;
        }

        public MockVariableService SetValue<T>(string name, T value = null)
            where T : class
        {
            // Dynamically build expression to bind only supplied parameters.
            var args = new Expression[]
            {
                Expression.Constant(name),
                Expression.Call(typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(T))),
            };

            if (value != null)
            {
                args[1] = Expression.Constant(value);
            }

            var param = Expression.Parameter(typeof(IVariableService), "x");
            Expression<Action<IVariableService>> expression = Expression.Lambda<Action<IVariableService>>(
                Expression.Call(param, typeof(IVariableService).GetMethod(nameof(IVariableService.SetValue)).MakeGenericMethod(typeof(T)), args),
                param);

            Mock.Setup(expression)
                .Verifiable();

            return this;
        }

        T IVariableService.GetValue<T>(string name)
        {
            return Object.GetValue<T>(name);
        }

        void IVariableService.SetValue<T>(string name, T value)
        {
            Object.SetValue(name, value);
        }
    }
}
