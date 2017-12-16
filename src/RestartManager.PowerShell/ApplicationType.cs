// <copyright file="ApplicationType.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    /// <summary>
    /// The type of application.
    /// </summary>
    public enum ApplicationType
    {
        /// <summary>
        /// The application cannot be classified as any other type.
        /// An application of this type can only be shut down by a forced shutdown.
        /// </summary>
        Unknown,

        /// <summary>
        /// A Windows application run as a stand-alone process that displays a top-level window.
        /// </summary>
        MainWindow,

        /// <summary>
        /// A Windows application that does not run as a stand-alone process and does not display a top-level window.
        /// </summary>
        OtherWindow,

        /// <summary>
        /// The application is a Windows service.
        /// </summary>
        Service,

        /// <summary>
        /// The application is Windows Explorer.
        /// </summary>
        Explorer,

        /// <summary>
        /// The application is a stand-alone console application.
        /// </summary>
        Console,

        /// <summary>
        /// A system restart is required to complete the installation because a process cannot be shut down.
        /// </summary>
        /// <remarks>
        /// The process cannot be shut down because of the following reasons:
        /// <list type="bullet">
        /// <item><description>The process may be a critical process.</description></item>
        /// <item><description>The current user may not have permission to shut down the process.</description></item>
        /// <item><description>The process may belong to the primary installer that started the Restart Manager.</description></item>
        /// </list>
        /// </remarks>
        Critical = 1000,
    }
}
