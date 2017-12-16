// <copyright file="ProcessInfo.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Information about a process from the Restart Manager.
    /// </summary>
    public class ProcessInfo : IProcessInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInfo"/> class.
        /// </summary>
        /// <param name="process">The <see cref="RM_PROCESS_INFO"/> to adapt.</param>
        /// <param name="reason">The <see cref="RebootReason"/> of why the computer must be restarted.</param>
        internal ProcessInfo(RM_PROCESS_INFO process, RebootReason reason)
        {
            var fileTime = ((long)process.Process.ProcessStartTime.dwHighDateTime << 32) + process.Process.ProcessStartTime.dwLowDateTime;

            Id = process.Process.dwProcessId;
            StartTime = DateTimeOffset.FromFileTime(fileTime);
            Description = process.strAppName;
            ServiceName = process.strServiceShortName;
            ApplicationType = process.ApplicationType;
            ApplicationStatus = process.AppStatus;
            IsRestartable = process.bRestartable;
            RebootReason = reason;
        }

        /// <inheritdoc/>
        public int Id { get; }

        /// <inheritdoc/>
        public DateTimeOffset StartTime { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public string ServiceName { get; }

        /// <inheritdoc/>
        public ApplicationType ApplicationType { get; }

        /// <inheritdoc/>
        public ApplicationStatus ApplicationStatus { get; }

        /// <inheritdoc/>
        public bool IsRestartable { get; }

        /// <inheritdoc/>
        public RebootReason RebootReason { get; }
    }
}
