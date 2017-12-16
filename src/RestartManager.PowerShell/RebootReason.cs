// <copyright file="RebootReason.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// The reasons a restart of the system is needed.
    /// </summary>
    [Flags]
    public enum RebootReason
    {
        /// <summary>
        /// A system restart is not required.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The current user does not have sufficient privileges to shut down one or more processes.
        /// </summary>
        PermissionDenied = 0x1,

        /// <summary>
        /// One or more processes are running in another Terminal Services session.
        /// </summary>
        SessionMismatch = 0x2,

        /// <summary>
        /// A system restart is needed because one or more processes to be shut down are critical processes.
        /// </summary>
        CriticalProcess = 0x4,

        /// <summary>
        /// A system restart is needed because one or more services to be shut down are critical services.
        /// </summary>
        CriticalService = 0x8,

        /// <summary>
        /// A system restart is needed because the current process must be shut down.
        /// </summary>
        DetectedSelf = 0x10,
    }
}
