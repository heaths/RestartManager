// <copyright file="ProcessAdapter.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Adapts a <see cref="Process"/>.
    /// </summary>
    internal class ProcessAdapter : IProcess
    {
        private readonly Process process;
        private readonly Lazy<DateTimeOffset> processStartTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessAdapter"/> class.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> to adapt.</param>
        internal ProcessAdapter(Process process)
        {
            Validate.NotNull(process, nameof(process));

            this.process = process;
            processStartTime = new Lazy<DateTimeOffset>(() => process.StartTime);
        }

        /// <inheritdoc/>
        public int Id => process.Id;

        /// <inheritdoc/>
        public DateTimeOffset StartTime => processStartTime.Value;
    }
}
