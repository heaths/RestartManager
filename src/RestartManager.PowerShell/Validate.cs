// <copyright file="Validate.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using RestartManager.Properties;

    /// <summary>
    /// Validates preconditions at runtime.
    /// </summary>
    internal static class Validate
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="value"/> is an empty string.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="paramName">The name of the parameter to validate.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is an empty string.</exception>
        public static void NotEmpty(string value, string paramName)
        {
            if (value != null && value.Length == 0)
            {
                throw new ArgumentException(Resources.Error_EmptyArgument, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null.
        /// </summary>
        /// <typeparam name="T">The reference type of object to validate.</typeparam>
        /// <param name="value">The object to validate.</param>
        /// <param name="paramName">The name of the parameter to validate.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static void NotNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the <paramref name="value"/> is null
        /// or an <see cref="ArgumentException"/> if the <paramref name="value"/> is an empty string.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="paramName">The name of the parameter to validate.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static void NotNullOrEmpty(string value, string paramName)
        {
            NotNull(value, paramName);
            NotEmpty(value, paramName);
        }
    }
}
