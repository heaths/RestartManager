// <copyright file="StopProcessCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;
    using RestartManager.Properties;

    /// <summary>
    /// The Stop-RestartManagerProcess cmdlet.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Stop, Nouns.RestartManagerProcess)]
    public class StopProcessCommand : Cmdlet
    {
        /// <summary>
        /// Gets or sets the <see cref="RestartManagerSession"/> with applications and services to stop.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public RestartManagerSession Session { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force applications and services to shut down.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to only shut down applications and services registered with Restart Manager.
        /// </summary>
        [Parameter]
        public SwitchParameter OnlyRegistered { get; set; }

        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            Session.ShutdownProgress += OnProgress;
            try
            {
                Session.ShutdownProcesses(force: Force, onlyRegistered: OnlyRegistered);
            }
            finally
            {
                Session.ShutdownProgress -= OnProgress;
            }
        }

        private void OnProgress(object sender, ProgressEventArgs e)
        {
            var progress = new ProgressRecord(GetHashCode(), Resources.Activity, Resources.ShutdownStatus)
            {
                PercentComplete = e.PercentComplete,
            };

            WriteProgress(progress);
        }
    }
}
