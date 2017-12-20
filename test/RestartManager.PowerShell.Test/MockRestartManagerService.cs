// <copyright file="MockRestartManagerService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System.Collections.Generic;

    /// <summary>
    /// Mock <see cref="IRestartManagerService"/>.
    /// </summary>
    internal class MockRestartManagerService
        : MockService<IRestartManagerService>, IRestartManagerService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockRestartManagerService"/> class.
        /// </summary>
        /// <param name="container">The parent <see cref="MockContainer"/>.</param>
        public MockRestartManagerService(MockContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.StartSession(out int, out string)"/>.
        /// </summary>
        /// <param name="sessionId">The session ID to return. The default is 0.</param>
        /// <param name="sessionKey">The session key to return. The default is "123abc".</param>
        /// <param name="error">The error to return. The default is 0 (no error).</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService StartSession(int sessionId = 0, string sessionKey = "123abc", int error = NativeMethods.ERROR_SUCCESS)
        {
            Mock.Setup(x => x.StartSession(out sessionId, out sessionKey))
                .Returns(error)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.EndSession(int)"/>.
        /// </summary>
        /// <param name="sessionId">The session ID to end.</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService EndSession(int sessionId = 0)
        {
            Mock.Setup(x => x.EndSession(sessionId))
                .Verifiable();

            return this;
        }

        void IRestartManagerService.EndSession(int sessionId)
        {
            Object.EndSession(sessionId);
        }

        int IRestartManagerService.GetProcesses(int sessionId, out int processesLengthRequired, ref int processesLength, RM_PROCESS_INFO[] processes, out RebootReason rebootReason)
        {
            return Object.GetProcesses(sessionId, out processesLengthRequired, ref processesLength, processes, out rebootReason);
        }

        int IRestartManagerService.RegisterResources(int sessionId, IEnumerable<string> files, IEnumerable<RM_UNIQUE_PROCESS> processes, IEnumerable<string> services)
        {
            return Object.RegisterResources(sessionId, files, processes, services);
        }

        int IRestartManagerService.RestartProcesses(int sessionId, RM_WRITE_STATUS_CALLBACK progress)
        {
            return Object.RestartProcesses(sessionId, progress);
        }

        int IRestartManagerService.ShutdownProcesses(int sessionId, RM_SHUTDOWN_TYPE shutdownType, RM_WRITE_STATUS_CALLBACK progress)
        {
            return Object.ShutdownProcesses(sessionId, shutdownType, progress);
        }

        int IRestartManagerService.StartSession(out int sessionId, out string sessionKey)
        {
            return Object.StartSession(out sessionId, out sessionKey);
        }
    }
}
