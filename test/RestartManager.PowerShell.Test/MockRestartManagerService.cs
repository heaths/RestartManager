// <copyright file="MockRestartManagerService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Moq;

    /// <summary>
    /// Mock <see cref="IRestartManagerService"/>.
    /// </summary>
    internal class MockRestartManagerService
        : MockService<IRestartManagerService>, IRestartManagerService
    {
        /// <summary>
        /// The default session ID.
        /// </summary>
        public const int DefaultSessionId = 0;

        /// <summary>
        /// The default session key.
        /// </summary>
        public const string DefaultSessionKey = "123abc";

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRestartManagerService"/> class.
        /// </summary>
        /// <param name="container">The parent <see cref="MockContainer"/>.</param>
        public MockRestartManagerService(MockContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets a collection of default <see cref="RM_PROCESS_INFO"/>.
        /// </summary>
        public static RM_PROCESS_INFO[] DefaultProcesses { get; } = new[]
        {
            new RM_PROCESS_INFO
            {
                ApplicationType = ApplicationType.MainWindow,
                AppStatus = ApplicationStatus.Running,
                bRestartable = true,
                strAppName = "WindowsApp",
                Process = new MockProcess(),
            },

            new RM_PROCESS_INFO
            {
                ApplicationType = ApplicationType.Console,
                AppStatus = ApplicationStatus.Running,
                bRestartable = true,
                strAppName = "ConsoleApp",
                Process = new MockProcess(),
            },

            new RM_PROCESS_INFO
            {
                ApplicationType = ApplicationType.Service,
                AppStatus = ApplicationStatus.Running,
                bRestartable = true,
                strAppName = "ServiceApp",
                strServiceShortName = "serviceapp",
                Process = new MockProcess(),
            },
        };

        /// <summary>
        /// Gets a collection of default <see cref="IProcessInfo"/>.
        /// </summary>
        /// <param name="rebootReason">The <see cref="RebootReason"/> for each <see cref="IProcessInfo"/>.</param>
        /// <returns>A collection of default <see cref="IProcessInfo"/> from <see cref="DefaultProcesses"/>.</returns>
        public static IEnumerable<IProcessInfo> GetDefaultProcessesInfo(RebootReason rebootReason = RebootReason.None) =>
            DefaultProcesses.Select(process => new ProcessInfo(process, rebootReason));

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.StartSession(out int, out string)"/>.
        /// </summary>
        /// <param name="sessionId">Optional session ID to return. The default is 0.</param>
        /// <param name="sessionKey">Optional session key to return. The default is "123abc".</param>
        /// <param name="error">Optional error to return. The default is 0 (no error).</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService StartSession(int sessionId = DefaultSessionId, string sessionKey = DefaultSessionKey, int error = NativeMethods.ERROR_SUCCESS)
        {
            Mock.Setup(x => x.StartSession(out sessionId, out sessionKey))
                .Returns(error)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.RegisterResources(int, IEnumerable{string}, IEnumerable{RM_UNIQUE_PROCESS}, IEnumerable{string})"/>.
        /// </summary>
        /// <param name="sessionId">Optional session ID to use. The default is 0.</param>
        /// <param name="files">Optional collection of files to register. The default is any.</param>
        /// <param name="processes">Optional collection of processes to register. The default is any.</param>
        /// <param name="services">Optional collection of service names to register. The default is any.</param>
        /// <param name="error">Optional error to return. The default is 0 (no error).</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService RegisterResources(int sessionId = DefaultSessionId, IEnumerable<string> files = null, IEnumerable<RM_UNIQUE_PROCESS> processes = null, IEnumerable<string> services = null, int error = NativeMethods.ERROR_SUCCESS)
        {
            // Dynamically build expression to bind only supplied parameters.
            var args = new Expression[]
            {
                Expression.Constant(sessionId),
                Expression.Call(typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(IEnumerable<string>))),
                Expression.Call(typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(IEnumerable<RM_UNIQUE_PROCESS>))),
                Expression.Call(typeof(It).GetMethod(nameof(It.IsAny), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(IEnumerable<string>))),
            };

            if (!files.NullOrEmpty())
            {
                args[1] = Expression.Constant(files);
            }

            if (!processes.NullOrEmpty())
            {
                args[2] = Expression.Constant(processes);
            }

            if (!services.NullOrEmpty())
            {
                args[3] = Expression.Constant(services);
            }

            var param = Expression.Parameter(typeof(IRestartManagerService), "x");
            Expression<Func<IRestartManagerService, int>> expression = Expression.Lambda<Func<IRestartManagerService, int>>(
                Expression.Call(param, typeof(IRestartManagerService).GetMethod(nameof(IRestartManagerService.RegisterResources)), args),
                param);

            Mock.Setup(expression)
                .Returns(error)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.GetProcesses(int, out int, ref int, RM_PROCESS_INFO[], out RebootReason)"/>.
        /// </summary>
        /// <param name="sessionId">Optional session ID to use. The default is 0.</param>
        /// <param name="processes">Optional array of <see cref="RM_PROCESS_INFO"/>. The default is <see cref="DefaultProcesses"/>.</param>
        /// <param name="rebootReason">Optional <see cref="RebootReason"/>. The default is <see cref="RebootReason.None"/>.</param>
        /// <param name="error">Optional error code to return for the first call. The default is 0 (no error).</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService GetProcesses(int sessionId = DefaultSessionId, RM_PROCESS_INFO[] processes = null, RebootReason rebootReason = RebootReason.None, int error = NativeMethods.ERROR_SUCCESS)
        {
            processes = processes ?? DefaultProcesses;

            var processesLengthRequired = processes.Length;
            var processesLength = 0;

            var sequence = new MockSequence();
            Mock.Setup(x => x.GetProcesses(sessionId, out processesLengthRequired, ref processesLength, null, out rebootReason))
                .Returns(error == NativeMethods.ERROR_SUCCESS ? NativeMethods.ERROR_MORE_DATA : error)
                .Verifiable();

            processesLength = processesLengthRequired;
            Mock.Setup(x => x.GetProcesses(sessionId, out processesLengthRequired, ref processesLength, It.IsAny<RM_PROCESS_INFO[]>(), out rebootReason))
                .OutCallback((int sessionId_, out int processesLengthRequired_, ref int processesLengt_, RM_PROCESS_INFO[] processes_, out RebootReason rebootReason_) =>
                {
                    processesLengthRequired_ = processesLengthRequired;
                    rebootReason_ = rebootReason;

                    processes.CopyTo(processes_, 0);
                })
                .Returns(NativeMethods.ERROR_SUCCESS)
                .Verifiable();

            return this;
        }

        /// <summary>
        /// Mocks <see cref="IRestartManagerService.EndSession(int)"/>.
        /// </summary>
        /// <param name="sessionId">Optional session ID to end. The default is 0.</param>
        /// <returns>The current instance for fluent calls.</returns>
        public MockRestartManagerService EndSession(int sessionId = DefaultSessionId)
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

        private class MockProcess : IProcess
        {
            private static int counter = 0;

            public int Id => ++counter;

            public DateTimeOffset StartTime => DateTimeOffset.Now;

            public static implicit operator RM_UNIQUE_PROCESS(MockProcess process)
            {
                return new RM_UNIQUE_PROCESS(process);
            }
        }
    }
}
