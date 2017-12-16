// <copyright file="NativeMethods.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1600 // Elements must be documented

    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using ComTypes = System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// Native methods.
    /// </summary>
    internal static class NativeMethods
    {
        internal const int ERROR_SUCCESS = 0;
        internal const int ERROR_OUTOFMEMORY = 14;
        internal const int ERROR_BAD_ARGUMENTS = 160;
        internal const int ERROR_MORE_DATA = 234;
        internal const int ERROR_MAX_SESSIONS_REACHED = 353;

        internal const int CCH_RM_SESSION_KEY = 32;
        internal const int CCH_RM_MAX_APP_NAME = 255;
        internal const int CCH_RM_MAX_SVC_NAME = 63;

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmStartSession(
            [MarshalAs(UnmanagedType.U4)] out int pSessionHandle,
            [MarshalAs(UnmanagedType.U4)] int dwSessionFlags,
            [Out] StringBuilder strSessionKey);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmRegisterResources(
            [MarshalAs(UnmanagedType.U4)] int dwSessionHandle,
            [MarshalAs(UnmanagedType.U4)] int nFiles,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.LPWStr)] string[] rgsFilenames,
            [MarshalAs(UnmanagedType.U4)] int nApplications,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3, ArraySubType = UnmanagedType.LPWStr)] RM_UNIQUE_PROCESS[] rgApplications,
            [MarshalAs(UnmanagedType.U4)] int nServices,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5, ArraySubType = UnmanagedType.LPWStr)] string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmGetList(
            [MarshalAs(UnmanagedType.U4)] int dwSessionHandle,
            [MarshalAs(UnmanagedType.U4)] out int pnProcInfoNeeded,
            [MarshalAs(UnmanagedType.U4)] ref int pnProcInfo,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out] RM_PROCESS_INFO[] rgAffectedApps,
            [MarshalAs(UnmanagedType.U4)] out RebootReason lpdwRebootReasons);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmShutdown(
            [MarshalAs(UnmanagedType.U4)] int dwSessionHandle,
            [MarshalAs(UnmanagedType.U4)] RM_SHUTDOWN_TYPE lActionFlags,
            [MarshalAs(UnmanagedType.FunctionPtr)] RM_WRITE_STATUS_CALLBACK fnStatus);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmRestart(
            [MarshalAs(UnmanagedType.U4)] int dwSessionHandle,
            [MarshalAs(UnmanagedType.U4)] int dwRestartFlags,
            [MarshalAs(UnmanagedType.FunctionPtr)] RM_WRITE_STATUS_CALLBACK fnStatus);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        internal static extern int RmEndSession(
            [MarshalAs(UnmanagedType.U4)] int dwSessionHandle);
    }

    internal delegate void RM_WRITE_STATUS_CALLBACK(
        [MarshalAs(UnmanagedType.U4)] int nPercentComplete);

    [Flags]
    internal enum RM_SHUTDOWN_TYPE
    {
        /// <summary>
        /// Force unresponsive applications and services to shut down after the timeout period.
        /// </summary>
        RmForceShutdown = 0x1,

        /// <summary>
        /// Shut down applications if and only if all the applications have been registered for restart.
        /// </summary>
        RmShutdownOnlyRegistered = 0x10,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RM_UNIQUE_PROCESS
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RM_UNIQUE_PROCESS"/> struct.
        /// </summary>
        /// <param name="process">The <see cref="IProcess"/> to convert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="process"/> is null.</exception>
        internal RM_UNIQUE_PROCESS(IProcess process)
        {
            Validate.NotNull(process, nameof(process));

            var startTime = process.StartTime.ToFileTime();
            var ft = new ComTypes.FILETIME
            {
                dwHighDateTime = (int)(startTime >> 32),
                dwLowDateTime = (int)(startTime & 0xffffffff),
            };

            dwProcessId = process.Id;
            ProcessStartTime = ft;
        }

        [MarshalAs(UnmanagedType.U4)]
        internal int dwProcessId;

        internal ComTypes.FILETIME ProcessStartTime;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct RM_PROCESS_INFO
    {
        internal RM_UNIQUE_PROCESS Process;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.CCH_RM_MAX_APP_NAME + 1)]
        internal string strAppName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.CCH_RM_MAX_SVC_NAME + 1)]
        internal string strServiceShortName;

        [MarshalAs(UnmanagedType.U4)]
        internal ApplicationType ApplicationType;

        [MarshalAs(UnmanagedType.U4)]
        internal ApplicationStatus AppStatus;

        [MarshalAs(UnmanagedType.U4)]
        internal int TSSessionId;

        [MarshalAs(UnmanagedType.Bool)]
        internal bool bRestartable;
    }

#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1201 // Elements must appear in the correct order
}
