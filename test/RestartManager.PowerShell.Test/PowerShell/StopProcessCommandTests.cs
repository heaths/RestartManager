// <copyright file="StopProcessCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation;
    using Moq;
    using Xunit;

    [Collection(RunspaceCollection.DefinitionName)]
    public class StopProcessCommandTests
    {
        private static readonly string CommandName = "Stop-RestartManagerProcess";
        private readonly RunspaceFixture fixture;

        public StopProcessCommandTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void No_Session_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .GetValue<RestartManagerSession>(SessionManager.VariableName, () => null)
                    .Pop();

            using (fixture.UseServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<NoSessionException>(ex.InnerException);
            }

            services.Verify();
        }

        [Fact]
        public void Force_Parameter()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .ShutdownProcesses(force: true)
                    .Pop();

            var onShutdownProgress = 0;
            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Force")
                    .AddParameter("Session", session);

                sut.Streams.Progress.DataAdded += (source, args) => ++onShutdownProgress;
                sut.Invoke();
            }

            services.Verify();
            Assert.Equal(3, onShutdownProgress);
        }

        [Fact]
        public void OnlyRegistered_Parameter()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .ShutdownProcesses(onlyRegistered: true)
                    .Pop();

            var onShutdownProgress = 0;
            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("OnlyRegistered")
                    .AddParameter("Session", session);

                sut.Streams.Progress.DataAdded += (source, args) => ++onShutdownProgress;
                sut.Invoke();
            }

            services.Verify();
            Assert.Equal(3, onShutdownProgress);
        }

        [Fact]
        public void Force_OnlyRegistered_Parameter()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .ShutdownProcesses(force: true, onlyRegistered: true)
                    .Pop();

            var onShutdownProgress = 0;
            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Force")
                    .AddParameter("OnlyRegistered")
                    .AddParameter("Session", session);

                sut.Streams.Progress.DataAdded += (source, args) => ++onShutdownProgress;
                sut.Invoke();
            }

            services.Verify();
            Assert.Equal(3, onShutdownProgress);
        }

        [Fact]
        public void Session_Variable()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .ShutdownProcesses()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s))
                    .Pop();

            var onShutdownProgress = 0;
            using (fixture.UseServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddStatement()
                    .AddCommand("Stop-RestartManagerSession");

                sut.Streams.Progress.DataAdded += (source, args) => ++onShutdownProgress;
                sut.Invoke();
            }

            services.Verify();
            Assert.Equal(3, onShutdownProgress);
        }

        [Fact]
        public void OnError()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .ShutdownProcesses(error: NativeMethods.ERROR_OUTOFMEMORY)
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<OutOfMemoryException>(ex.InnerException);
            }

            services.Verify();
        }
    }
}
