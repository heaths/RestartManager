// <copyright file="RegisterResourceCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Diagnostics;
    using System.Management.Automation;
    using Moq;
    using RestartManager.Properties;
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
        public void Register_Path_Parameter()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Path", path)
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void Register_Path_Argument()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddArgument(path)
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void Register_LiteralPath_Pipeline()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand("Get-Item")
                    .AddArgument(path)
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke();
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
                .Push<IRestartManagerService>()
                    .RegisterResources(processes: new[] { uniqueProcess })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Process", process)
                    .AddParameter("Session", session)
                    .Invoke();
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
                .Push<IRestartManagerService>()
                    .RegisterResources(processes: new[] { uniqueProcess })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke(new[] { process });
            }

            services.Verify();
        }

        [Fact]
        public void Register_Service_Parameter()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(services: new[] { "ServiceApp" })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("ServiceName", "ServiceApp")
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void Register_Service_Pipeline()
        {
            var service = new
            {
                ServiceName = "ServiceApp",
            };

            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(services: new[] { "ServiceApp" })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke(new[] { service });
            }

            services.Verify();
        }

        [Fact]
        public void Register_All_Parameter()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var process = Process.GetCurrentProcess();
            var processInfo = new ProcessAdapter(process);
            var uniqueProcess = new RM_UNIQUE_PROCESS(processInfo);

            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path }, processes: new[] { uniqueProcess }, services: new[] { "ServiceApp" })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Path", path)
                    .AddParameter("Process", process)
                    .AddParameter("ServiceName", "ServiceApp")
                    .AddParameter("Session", session)
                    .Invoke();
            }

            services.Verify();
        }

        [Fact]
        public void Register_All_Pipeline()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var file = new
            {
                Path = path,
                LiteralPath = $@"Microsoft.PowerShell.Core\FileSystem::{path}",
            };
            var process = Process.GetCurrentProcess();
            var processInfo = new ProcessAdapter(process);
            var uniqueProcess = new RM_UNIQUE_PROCESS(processInfo);
            var service = new
            {
                ServiceName = "ServiceApp",
            };

            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path }, processes: new[] { uniqueProcess }, services: new[] { "ServiceApp" })
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Session", session)
                    .Invoke(new object[] { file, process, service });
            }

            services.Verify();
        }

        [Fact]
        public void Session_Variable()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path })
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s))
                    .Pop();

            using (fixture.UseServices(services))
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

        [Fact]
        public void OnError()
        {
            var path = typeof(RegisterResourceCommand).Assembly.Location;
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .RegisterResources(files: new[] { path }, error: NativeMethods.ERROR_OUTOFMEMORY)
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Path", path)
                    .AddParameter("Session", session);

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<OutOfMemoryException>(ex.InnerException);
            }

            services.Verify();
        }

        [Fact]
        public void NonFile_Path_Warns()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .Pop();

            using (var session = new RestartManagerSession(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("PSPath", @"Microsoft.PowerShell.Core\Registry::HKEY_LOCAL_MACHINE\Software")
                    .AddParameter("Session", session);

                var output = sut.Invoke();
                Assert.Empty(output);

                var warnings = sut.Streams.Warning;
                Assert.Collection(warnings, x => Assert.Equal(Resources.Warning_NoFiles, x.Message));
            }

            services.Verify();
        }
    }
}
