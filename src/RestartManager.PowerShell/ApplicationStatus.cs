// <copyright file="ApplicationStatus.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// The status of the application.
    /// </summary>
    [Flags]
    public enum ApplicationStatus
    {
        /// <summary>
        /// The application is in a state that is not described by any other enumerated state.
        /// </summary>
        Unknown = 0x0,

        /// <summary>
        /// The application is currently running.
        /// </summary>
        Running = 0x1,

        /// <summary>
        /// The Restart Manager has stopped the application.
        /// </summary>
        Stopped = 0x2,

        /// <summary>
        /// An action outside the Restart Manager has stopped the application.
        /// </summary>
        StoppedOther = 0x4,

        /// <summary>
        /// The Restart Manager has restarted the application.
        /// </summary>
        Restarted = 0x8,

        /// <summary>
        /// The Restart Manager encountered an error when stopping the application.
        /// </summary>
        ErrorOnStop = 0x10,

        /// <summary>
        /// The Restart Manager encountered an error when restarting the application.
        /// </summary>
        ErrorOnRestart = 0x20,

        /// <summary>
        /// Shutdown is masked by a filter.
        /// </summary>
        ShutdownMasked = 0x40,

        /// <summary>
        /// Restart is masked by a filter.
        /// </summary>
        RestartMasked = 0x80,
    }
}
