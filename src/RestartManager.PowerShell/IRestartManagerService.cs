// <copyright file="IRestartManagerService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System.Collections.Generic;

    /// <summary>
    /// Service interface for the Restart Manager.
    /// </summary>
    internal interface IRestartManagerService
    {
        /// <summary>
        /// Starts a Restart Manager session.
        /// </summary>
        /// <param name="sessionId">The session identity.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <returns>A Win32 return code.</returns>
        int StartSession(out int sessionId, out string sessionKey);

        /// <summary>
        /// Registers one or more resources with the Restart Manager.
        /// </summary>
        /// <param name="sessionId">The session identity.</param>
        /// <param name="files">Optional array of file paths.</param>
        /// <param name="processes">Optional array of processes.</param>
        /// <param name="services">Optional array of service names.</param>
        /// <returns>A Win32 return code.</returns>
        int RegisterResources(int sessionId, IEnumerable<string> files, IEnumerable<RM_UNIQUE_PROCESS> processes, IEnumerable<string> services);

        /// <summary>
        /// Shuts down application and services for the registered resources.
        /// </summary>
        /// <param name="sessionId">The session identity.</param>
        /// <param name="shutdownType">The <see cref="RM_SHUTDOWN_TYPE"/> to perform when shutting down applications and services.</param>
        /// <param name="progress">The <see cref="RM_WRITE_STATUS_CALLBACK"/> callback for progress when shutting down applications and services. Can be null.</param>
        /// <returns>A Win32 return code.</returns>
        int ShutdownProcesses(int sessionId, RM_SHUTDOWN_TYPE shutdownType, RM_WRITE_STATUS_CALLBACK progress);

        /// <summary>
        /// Restarts applications and services previously shut down.
        /// </summary>
        /// <param name="sessionId">The session identity.</param>
        /// <param name="progress">The <see cref="RM_WRITE_STATUS_CALLBACK"/> callback for progress when shutting down applications and services. Can be null.</param>
        /// <returns>A Win32 return code.</returns>
        int RestartProcesses(int sessionId, RM_WRITE_STATUS_CALLBACK progress);

        /// <summary>
        /// Ends a Restart Manager session.
        /// </summary>
        /// <param name="sessionId">The session identity.</param>
        void EndSession(int sessionId);
    }
}
