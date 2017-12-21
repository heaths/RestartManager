// <copyright file="RegisterResourceCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Diagnostics;
    using System.Management.Automation;
    using Moq;
    using Xunit;

    [Collection(RunspaceCollection.DefinitionName)]
    public class RegisterResourceCommandTests
    {
        private static readonly string CommandName = "Register-RestartManagerResource";
        private readonly RunspaceFixture fixture;

        public RegisterResourceCommandTests(RunspaceFixture fixture)
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

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<NoSessionException>(ex.InnerException);
            }

            services.Verify();
        }

        [Fact]
        public void Register_Path_Parameter()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(files: new[] { path })
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                using (fixture.SetServices(services))
                {
                    fixture.Create()
                        .AddCommand(CommandName)
                        .AddParameter("Path", path)
                        .AddParameter("Session", session)
                        .Invoke();
                }
            }

            services.Verify();
        }

        [Fact]
        public void Register_Path_Argument()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(files: new[] { path })
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                using (fixture.SetServices(services))
                {
                    fixture.Create()
                        .AddCommand(CommandName)
                        .AddArgument(path)
                        .AddParameter("Session", session)
                        .Invoke();
                }
            }

            services.Verify();
        }

        [Fact]
        public void Register_LiteralPath_Pipeline()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(files: new[] { path })
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                using (fixture.SetServices(services))
                {
                    fixture.Create()
                        .AddCommand("Get-Item")
                        .AddArgument(path)
                        .AddCommand(CommandName)
                        .AddParameter("Session", session)
                        .Invoke();
                }
            }

            services.Verify();
        }

        [Fact]
        public void Register_Process_Parameter()
        {
            var process = Process.GetCurrentProcess();
            var processInfo = new ProcessAdapter(process);
            var uniqueProcess = new RM_UNIQUE_PROCESS(processInfo);

            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(processes: new[] { uniqueProcess })
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                using (fixture.SetServices(services))
                {
                    fixture.Create()
                        .AddCommand(CommandName)
                        .AddParameter("Process", process)
                        .AddParameter("Session", session)
                        .Invoke();
                }
            }

            services.Verify();
        }

        [Fact]
        public void Register_Process_Pipeline()
        {
            var process = Process.GetCurrentProcess();
            var processInfo = new ProcessAdapter(process);
            var uniqueProcess = new RM_UNIQUE_PROCESS(processInfo);

            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(processes: new[] { uniqueProcess })
                    .EndSession()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                using (fixture.SetServices(services))
                {
                    fixture.Create()
                        .AddCommand(CommandName)
                        .AddParameter("Session", session)
                        .Invoke(new[] { process });
                }
            }

            services.Verify();
        }

        [Fact]
        public void Session_Variable()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .RegisterResources(files: new[] { path })
                    .EndSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s))
                    .Pop();

            using (fixture.SetServices(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Path", path)
                    .AddStatement()
                    .AddCommand("Stop-RestartManagerSession")
                    .Invoke();
            }

            services.Verify();
        }
    }
}
