// <copyright file="ITestableHost.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation.Host;

    /// <summary>
    /// Indicates that a <see cref="PSHost"/> may contain an <see cref="IServiceProvider"/> for testing.
    /// </summary>
    /// <remarks>
    /// Rather than use a variable that may cause problems at runtime, require that the <see cref="PSHost"/>
    /// provide an <see cref="IServiceProvider"/>.
    /// </remarks>
    internal interface ITestableHost
    {
        /// <summary>
        /// Gets the services provided by the host.
        /// </summary>
        IServiceProvider Services { get; }
    }
}
