// <copyright file="WindowsRestartManagerService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The Windows Restart Manager service.
    /// </summary>
    internal class WindowsRestartManagerService : IRestartManagerService
    {
        /// <summary>
        /// Gets a singleton instance of <see cref="WindowsRestartManagerService"/>.
        /// </summary>
        internal static readonly IRestartManagerService Default = new WindowsRestartManagerService();

        /// <inheritdoc/>
        public int StartSession(out int sessionId, out string sessionKey)
        {
            var buffer = new StringBuilder(NativeMethods.CCH_RM_SESSION_KEY);

            var error = NativeMethods.RmStartSession(out var id, 0, buffer);
            if (error == NativeMethods.ERROR_SUCCESS)
            {
                sessionId = id;
                sessionKey = buffer.ToString();
            }
            else
            {
                sessionId = 0;
                sessionKey = null;
            }

            return error;
        }

        /// <inheritdoc/>
        public int Register(int sessionId, IEnumerable<string> files, IEnumerable<RM_UNIQUE_PROCESS> processes, IEnumerable<string> services) =>
            NativeMethods.RmRegisterResources(sessionId, files?.Count() ?? 0, files?.ToArray(), processes?.Count() ?? 0, processes?.ToArray(), services?.Count() ?? 0, services?.ToArray());

        /// <inheritdoc/>
        public void EndSession(int sessionId) => NativeMethods.RmEndSession(sessionId);
    }
}
