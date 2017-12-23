// <copyright file="RunspaceFixture.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    /// <summary>
    /// A test fixture for testing cmdlets and functions.
    /// </summary>
    public sealed class RunspaceFixture : IDisposable
    {
        private readonly TestableHost host;
        private readonly Runspace runspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunspaceFixture"/> class.
        /// </summary>
        public RunspaceFixture()
        {
            host = new TestableHost();

            var state = InitialSessionState.CreateDefault2();
            state.AuthorizationManager = null;
            state.LanguageMode = PSLanguageMode.RestrictedLanguage;
            state.ImportPSModule(new[] { @".\RestartManager.psd1" });

            runspace = RunspaceFactory.CreateRunspace(host, state);
            runspace.Open();
        }

        /// <summary>
        /// Creates a new <see cref="PowerShell"/> instance.
        /// </summary>
        /// <param name="throwOnError">Throw <see cref="RunspaceException"/>if error is written to pipeline.</param>
        /// <returns>A new <see cref="PowerShell"/> instance.</returns>
        public PowerShell Create(bool throwOnError = true)
        {
            var ps = PowerShell.Create();
            ps.Runspace = runspace;

            if (throwOnError)
            {
                var errors = ps.Streams.Error;
                errors.DataAdded += (source, args) =>
                {
                    var error = errors?[args.Index];
                    throw new RunspaceException(error);
                };
            }

            return ps;
        }

        /// <summary>
        /// Disposes the <see cref="RunspaceFixture"/> instance.
        /// </summary>
        public void Dispose()
        {
            runspace.Dispose();
        }

        /// <summary>
        /// Sets the <paramref name="services"/> on the <see cref="TestableHost"/> for the current <see cref="Runspace"/>.
        /// </summary>
        /// <param name="services">The <see cref="RestartManager.IServiceProvider"/> to set on the <see cref="TestableHost"/>.</param>
        /// <returns>An <see cref="IDisposable"/> object that will unset the <paramref name="services"/> when disposed.</returns>
        internal IDisposable UseServices(RestartManager.IServiceProvider services)
        {
            return new ServiceScope(host, services);
        }

        private class ServiceScope : IDisposable
        {
            private readonly TestableHost host;

            public ServiceScope(TestableHost host, RestartManager.IServiceProvider services)
            {
                this.host = host;
                host.Services = services;
            }

            public void Dispose()
            {
                host.Services = null;
            }
        }
    }
}
