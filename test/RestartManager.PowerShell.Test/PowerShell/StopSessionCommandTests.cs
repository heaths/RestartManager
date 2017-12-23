// <copyright file="StopSessionCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using Moq;
    using Xunit;

    [Collection(RunspaceCollection.DefinitionName)]
    public class StopSessionCommandTests
    {
        private static readonly string CommandName = "Stop-RestartManagerSession";
        private readonly RunspaceFixture fixture;

        public StopSessionCommandTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Session_Disposed()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .Pop();

            var session = new RestartManagerSession(services);
            session.Dispose();

            services.Verify<IRestartManagerService>(x => x.EndSession(0), Times.Once);

            fixture.Create()
                .AddCommand(CommandName)
                .AddParameter("Session", session)
                .Invoke();

            services.Verify<IRestartManagerService>(x => x.EndSession(0), Times.Once);
        }

        [Fact]
        public void Ends_Session()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify<IRestartManagerService>(x => x.EndSession(0), Times.Once);
        }

        [Fact]
        public void No_Session()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .GetValue<RestartManagerSession>(SessionManager.VariableName, () => null)
                    .Pop();

            using (fixture.UseServices(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void Ends_Session_Variable()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s))
                    .Pop();

            using (fixture.UseServices(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void EndSession_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .Pop();

            var restartManagerServiceMock = services.Get<IRestartManagerService>();
            restartManagerServiceMock.Setup(x => x.EndSession(0))
                .Throws(new ObjectDisposedException(nameof(RestartManagerSession)))
                .Verifiable();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify();
        }
    }
}
