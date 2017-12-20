// <copyright file="SessionCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;

    /// <summary>
    /// Base class for cmdlets that accept a <see cref="RestartManagerSession"/>.
    /// </summary>
    public abstract class SessionCommand : PSCmdlet
    {
        private RestartManagerSession session = null;

        /// <summary>
        /// Gets or sets the <see cref="RestartManagerSession"/> for the command.
        /// </summary>
        [Parameter]
        [ValidateNotNull]
        public RestartManagerSession Session
        {
            get => SessionManager.GetParameterOrVariable(Services, ref session, SessionState.PSVariable);
            set => session = value;
        }

        /// <summary>
        /// Gets an <see cref="IServiceProvider"/> from an <see cref="ITestableHost"/>.
        /// </summary>
        internal IServiceProvider Services => Host.GetServices();
    }
}
