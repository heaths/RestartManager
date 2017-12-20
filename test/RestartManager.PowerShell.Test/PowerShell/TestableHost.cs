// <copyright file="TestableHost.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Globalization;
    using System.Management.Automation.Host;
    using System.Reflection;

    /// <summary>
    /// A <see cref="PSHost"/> for use with the <see cref="RunspaceFixture"/>.
    /// </summary>
    internal sealed class TestableHost : PSHost, ITestableHost
    {
        /// <inheritdoc/>
        public override CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        /// <inheritdoc/>
        public override CultureInfo CurrentUICulture => CultureInfo.CurrentUICulture;

        /// <inheritdoc/>
        public override Guid InstanceId { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public override string Name => nameof(TestableHost);

        /// <inheritdoc/>
        public override PSHostUserInterface UI => null;

        /// <inheritdoc/>
        public override Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        /// <inheritdoc/>
        public RestartManager.IServiceProvider Services { get; set; }

        /// <summary>
        /// Gets the last exit code passed to <see cref="SetShouldExit(int)"/>.
        /// </summary>
        public int LastExitCode { get; private set; }

        /// <inheritdoc/>
        public override void EnterNestedPrompt()
        {
        }

        /// <inheritdoc/>
        public override void ExitNestedPrompt()
        {
        }

        /// <inheritdoc/>
        public override void NotifyBeginApplication()
        {
        }

        /// <inheritdoc/>
        public override void NotifyEndApplication()
        {
        }

        /// <inheritdoc/>
        public override void SetShouldExit(int exitCode)
        {
            LastExitCode = exitCode;
        }
    }
}
