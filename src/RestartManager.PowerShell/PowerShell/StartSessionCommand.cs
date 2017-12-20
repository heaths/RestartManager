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
    public class StartSessionCommand : PSCmdlet
    {
        /// <summary>
        /// Gets or sets a value indicating whether to return the <see cref="RestartManagerSession"/> in addition to setting the session variable.
        /// </summary>
        [Parameter]
        public SwitchParameter PassThru { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to dispose any current <see cref="RestartManagerSession"/> and create a new session.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        private IServiceProvider Services => Host.GetServices();

        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            var session = SessionManager.GetVariable(Services, SessionState.PSVariable);
            if (session != null && !session.IsDisposed)
            {
                if (Force)
                {
                    session.Dispose();
                }
                else
                {
                    throw new ActiveSessionException();
                }
            }

            session = new RestartManagerSession(Services);
            SessionManager.SetVariable(Services, SessionState.PSVariable, session);

            if (PassThru)
            {
                WriteObject(session);
            }
        }
    }
}
