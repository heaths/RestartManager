// <copyright file="IProcess.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Represents a process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Gets the process ID.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the process start time (UTC).
        /// </summary>
        DateTimeOffset StartTime { get; }
    }
}
