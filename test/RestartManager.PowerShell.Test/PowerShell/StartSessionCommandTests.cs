// <copyright file="StartSessionCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Linq;
    using System.Management.Automation;
    using Moq;
    using Xunit;

    [Collection(RunspaceCollection.DefinitionName)]
    public class StartSessionCommandTests
    {
        private static readonly string CommandName = "Start-RestartManagerSession";
        private readonly RunspaceFixture fixture;

        public StartSessionCommandTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Creates_Session_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession(error: NativeMethods.ERROR_OUTOFMEMORY)
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue<RestartManagerSession>(SessionManager.VariableName, () => null)
                    .Pop();

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("PassThru");

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<OutOfMemoryException>(ex.InnerException);
            }
        }

        [Fact]
        public void Creates_Session()
        {
            var services = new MockContainer()
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue<RestartManagerSession>(SessionManager.VariableName, () => null)
                    .Pop();

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("PassThru");

                using (var session = sut.Invoke<RestartManagerSession>().Single())
                {
                    Assert.Equal(0, session.SessionId);
                    Assert.Equal("123abc", session.SessionKey);

                    services.Verify<IVariableService>(x => x.SetValue(SessionManager.VariableName, session));
                }
            }
        }

        [Fact]
        public void Overrides_Session_Throws()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => new RestartManagerSession(s))
                    .Pop();

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("PassThru");

                var ex = Assert.Throws<CmdletInvocationException>(() => sut.Invoke());
                Assert.IsType<ActiveSessionException>(ex.InnerException);
            }
        }

        [Fact]
        public void Overrides_Session()
        {
            RestartManagerSession original = null;
            var services = new MockContainer()
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => original = new RestartManagerSession(s))
                    .Pop();

            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("Force")
                    .AddParameter("PassThru");

                using (var session = sut.Invoke<RestartManagerSession>().Single())
                {
                    Assert.Equal(0, session.SessionId);
                    Assert.Equal("123abc", session.SessionKey);
                    Assert.NotSame(original, session);

                    services.Verify<IVariableService>(x => x.SetValue(SessionManager.VariableName, session));
                }
            }
        }

        [Fact]
        public void Overrides_Disposed_Session()
        {
            RestartManagerSession original = null;
            var services = new MockContainer()
                .Push<IRestartManagerService, MockRestartManagerService>()
                    .StartSession()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, s => original = new RestartManagerSession(s))
                    .Pop();

            original.Dispose();
            using (fixture.SetServices(services))
            {
                var sut = fixture.Create()
                    .AddCommand(CommandName)
                    .AddParameter("PassThru");

                using (var session = sut.Invoke<RestartManagerSession>().Single())
                {
                    Assert.Equal(0, session.SessionId);
                    Assert.Equal("123abc", session.SessionKey);
                    Assert.NotSame(original, session);

                    services.Verify<IVariableService>(x => x.SetValue(SessionManager.VariableName, session));
                }
            }
        }
    }
}
