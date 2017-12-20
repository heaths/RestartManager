// <copyright file="IVariableService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;

    /// <summary>
    /// A service for managing variables.
    /// </summary>
    internal interface IVariableService
    {
        /// <summary>
        /// Gets the value of a named variable.
        /// </summary>
        /// <typeparam name="T">The type of variable to get.</typeparam>
        /// <param name="name">The name of the variable to get.</param>
        /// <returns>The value of the named variable, or the default value of <typeparamref name="T"/> if not found.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        T GetValue<T>(string name)
            where T : class;

        /// <summary>
        /// Sets the value of the named variable.
        /// </summary>
        /// <typeparam name="T">The type of variable to set.</typeparam>
        /// <param name="name">The name of the variable to set.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        void SetValue<T>(string name, T value)
            where T : class;
    }
}
