// <copyright file="StartSessionCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;

    /// <summary>
    /// The Start-RestartManagerSession cmdlet.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, Nouns.RestartManagerSession)]
    public class StartSessionCommand : Cmdlet
    {
        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            var session = RestartManagerSession.Create(null);
            WriteObject(session);
        }
    }
}
