// <copyright file="StopSessionCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation;

    /// <summary>
    /// The Stop-RestartManagerSession cmdlet.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, Nouns.RestartManagerSession)]
    public class StopSessionCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets the <see cref="RestartManagerSession"/> to stop.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public RestartManagerSession Session { get; set; }

        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            try
            {
                Session?.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
