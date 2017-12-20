// <copyright file="Extensions.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation.Host;
    using System.Reflection;

    /// <summary>
    /// Extension methods.
    /// </summary>
    internal static class Extensions
    {
        private static Type internalHostType = null;
        private static PropertyInfo externalHostProperty = null;

        /// <summary>
        /// Gets an <see cref="RestartManager.IServiceProvider"/> from the <see cref="PSHost"/> if it implements <see cref="ITestableHost"/>.
        /// </summary>
        /// <param name="host">The <see cref="PSHost"/> to extend.</param>
        /// <returns>An <see cref="RestartManager.IServiceProvider"/> or null if not available.</returns>
        public static RestartManager.IServiceProvider GetServices(this PSHost host)
        {
            if (host is ITestableHost testable)
            {
                return testable.Services;
            }
            else if (GetExternalHost(host) is ITestableHost wrapped)
            {
                return wrapped.Services;
            }

            return null;
        }

        private static PSHost GetExternalHost(PSHost host)
        {
            if (host != null)
            {
                if (internalHostType == null)
                {
                    internalHostType = typeof(PSHost).Assembly.GetType("System.Management.Automation.Internal.Host.InternalHost");
                }

                if (externalHostProperty == null)
                {
                    externalHostProperty = internalHostType?.GetProperty("ExternalHost", BindingFlags.Instance | BindingFlags.NonPublic);
                }

                if (externalHostProperty != null && internalHostType.IsInstanceOfType(host))
                {
                    host = externalHostProperty.GetValue(host) as PSHost;
                }
            }

            return host;
        }
    }
}
