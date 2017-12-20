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
    public class StopSessionCommand : SessionCommand
    {
        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            try
            {
                Session?.Dispose();
            }
            catch (NoSessionException ex)
            {
                WriteDebug(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                WriteDebug(ex.Message);
            }
        }
    }
}
