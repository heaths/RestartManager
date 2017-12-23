// <copyright file="SessionManagerTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using Moq;
    using Xunit;

    public sealed class SessionManagerTests : IClassFixture<RunspaceFixture>, IDisposable
    {
        private readonly RunspaceFixture fixture;
        private readonly RestartManagerSession sessionService;
        private readonly RestartManagerSession sessionVariable;

        public SessionManagerTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;

            var services = new MockContainer()
                .Push<IRestartManagerService>()
                    .Pop();

            sessionService = new RestartManagerSession(services);
            sessionVariable = new RestartManagerSession(services);

            fixture.Variables.Set(SessionManager.VariableName, sessionVariable);
        }

        [Fact]
        public void GetVariable_Mocked()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, sessionService)
                    .Pop();

            var actual = SessionManager.GetVariable(services, fixture.Variables);
            Assert.Same(sessionService, actual);
        }

        [Fact]
        public void GetVariable_No_Service()
        {
            var services = new MockContainer();

            var actual = SessionManager.GetVariable(services, fixture.Variables);
            Assert.Same(sessionVariable, actual);
        }

        [Fact]
        public void GetVariable_No_Services()
        {
            var actual = SessionManager.GetVariable(null, fixture.Variables);
            Assert.Same(sessionVariable, actual);
        }

        [Fact]
        public void GetParameterOrVariable_Mocked()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, sessionService)
                    .Pop();

            RestartManagerSession session = null;
            var actual = SessionManager.GetParameterOrVariable(services, ref session, fixture.Variables);
            Assert.Same(sessionService, actual);
        }

        [Fact]
        public void GetParameterOrVariable_No_Service()
        {
            var services = new MockContainer();

            RestartManagerSession session = null;
            var actual = SessionManager.GetParameterOrVariable(services, ref session, fixture.Variables);
            Assert.Same(sessionVariable, actual);
        }

        [Fact]
        public void GetParameterOrVariable_No_Services()
        {
            RestartManagerSession session = null;
            var actual = SessionManager.GetParameterOrVariable(null, ref session, fixture.Variables);
            Assert.Same(sessionVariable, actual);
        }

        [Fact]
        public void GetParameterOrVariable_Field()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IRestartManagerService>()
                    .Pop()
                .Push<IVariableService, MockVariableService>()
                    .GetValue(SessionManager.VariableName, sessionService)
                    .Pop();

            var session = new RestartManagerSession(services);
            try
            {
                var actual = SessionManager.GetParameterOrVariable(services, ref session, fixture.Variables);
                Assert.Same(session, actual);
            }
            finally
            {
                session.Dispose();
            }
        }

        [Fact]
        public void SetVariable_Mocked()
        {
            var services = new MockContainer(MockBehavior.Strict)
                .Push<IVariableService, MockVariableService>()
                    .SetValue(SessionManager.VariableName, sessionService)
                    .Pop();

            SessionManager.SetVariable(services, fixture.Variables, sessionService);

            services.Verify();
        }

        [Fact]
        public void SetVariable_No_Service()
        {
            var services = new MockContainer();

            SessionManager.SetVariable(services, fixture.Variables, sessionVariable);

            var actual = fixture.Variables.Get(SessionManager.VariableName).Value as RestartManagerSession;
            Assert.Same(sessionVariable, actual);
        }

        [Fact]
        public void SetVariable_No_Services()
        {
            SessionManager.SetVariable(null, fixture.Variables, sessionVariable);

            var actual = fixture.Variables.Get(SessionManager.VariableName).Value as RestartManagerSession;
            Assert.Same(sessionVariable, actual);
        }

        public void Dispose()
        {
            sessionService.Dispose();
            sessionVariable.Dispose();
        }
    }
}
