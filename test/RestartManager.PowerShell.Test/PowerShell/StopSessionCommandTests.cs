// <copyright file="StopSessionCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
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
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .EndSession()
                    .Pop();

            var session = new RestartManagerSession(services, 0, "123abc");
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
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services, 0, "123abc"))
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

            using (fixture.SetServices(services))
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
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .EndSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s, 0, "123abc"))
                    .Pop();

            using (fixture.SetServices(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .Invoke();
            }

            services.Verify();
        }
    }
}
