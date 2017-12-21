// <copyright file="GetProcessCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;

    /// <summary>
    /// The Get-RestartManagerProcess cmdlet.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Nouns.RestartManagerProcess)]
    public class GetProcessCommand : SessionCommand
    {
        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            var processes = Session.GetProcesses();
            WriteObject(processes, true);
        }
    }
}
