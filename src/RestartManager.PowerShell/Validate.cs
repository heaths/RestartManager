// <copyright file="Validate.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Validates preconditions at runtime.
    /// </summary>
    internal static class Validate
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null.
        /// </summary>
        /// <typeparam name="T">The reference type of object to validate.</typeparam>
        /// <param name="value">The object to validate.</param>
        /// <param name="paramName">The name of the parameter to validate.</param>
        public static void NotNull<T>(T value, string paramName)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
