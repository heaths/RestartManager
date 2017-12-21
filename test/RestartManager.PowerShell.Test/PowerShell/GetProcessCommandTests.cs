// <copyright file="GetProcessCommandTests.cs" company="Heath Stewart">
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
    public class GetProcessCommandTests
    {
        private static readonly string CommandName = "Get-RestartManagerProcess";
        private readonly RunspaceFixture fixture;

        public GetProcessCommandTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ProcessRecord_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources()
                    .GetProcesses(error: NativeMethods.ERROR_OUTOFMEMORY)
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                session.RegisterResources(files: new[] { @"C:\ShouldNotExist.txt" });

                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<OutOfMemoryException>(ex.InnerException);
            }
        }

        [Fact]
        public void ProcessRecord()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources()
                    .GetProcesses()
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                session.RegisterResources(files: new[] { @"C:\ShouldNotExist.txt" });

                var output = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke<IProcessInfo>();

                var expected = MockRestartManagerService.GetDefaultProcessesInfo(RebootReason.None);
                Assert.Equal(expected, output, ProcessComparer.Default);
            }
        }

        [Fact]
        public void No_Session_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .GetValue<RestartManagerSession>(SessionManager.VariableName, () => null)
                    .Pop();

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<NoSessionException>(ex.InnerException);
            }
        }

        [Fact]
        public void Session_Variable()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources()
                    .GetProcesses()
                    .EndSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s =>
                    {
                        var session = new RestartManagerSession(s);
                        session.RegisterResources(files: new[] { @"C:\ShouldNotExist.txt" });

                        return session;
                    })
                    .Pop();

            using (fixture.SetServices(services))
            {
                var output = fixture.Create()
                    .AddCommand(CommandName)
                    .AddStatement()
                    .AddCommand("Stop-RestartManagerSession")
                    .Invoke<IProcessInfo>();

                var expected = MockRestartManagerService.GetDefaultProcessesInfo(RebootReason.None);
                Assert.Equal(expected, output, ProcessComparer.Default);
            }

            services.Verify();
        }
    }
}
