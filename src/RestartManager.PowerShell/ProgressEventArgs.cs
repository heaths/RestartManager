// <copyright file="ProgressEventArgs.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;

    /// <summary>
    /// Progress event information.
    /// </summary>
    internal class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="percentComplete">The progress completion in percent from 0 to 100.</param>
        internal ProgressEventArgs(int percentComplete)
        {
            PercentComplete = percentComplete;
        }

        /// <summary>
        /// Gets the process completion in percent from 0 to 100.
        /// </summary>
        internal int PercentComplete { get; }
    }
}
