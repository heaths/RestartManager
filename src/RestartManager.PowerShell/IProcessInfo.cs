// <copyright file="IProcessInfo.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    /// <summary>
    /// Process information from Restart Manager.
    /// </summary>
    public interface IProcessInfo : IProcess
    {
        /// <summary>
        /// Gets the description of the application or service.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the service name.
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Gets the <see cref="ApplicationType"/> of the application or service.
        /// </summary>
        ApplicationType ApplicationType { get; }

        /// <summary>
        /// Gets the <see cref="ApplicationStatus"/> of the application or service.
        /// </summary>
        ApplicationStatus ApplicationStatus { get; }

        /// <summary>
        /// Gets a value indicating whether the application can be restarted by the Restart Manager.
        /// </summary>
        /// <value>Always returns true for services.</value>
        bool IsRestartable { get; }

        /// <summary>
        /// Gets the <see cref="RebootReason"/> why the computer must be restarted.
        /// </summary>
        RebootReason RebootReason { get; }
    }
}
