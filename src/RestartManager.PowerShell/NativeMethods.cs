// <copyright file="NativeMethods.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
#pragma warning disable SA1201 // Elements must appear in the correct order
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
#pragma warning disable SA1600 // Elements must be documented

    using System;
    using System.Diagnostics;
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

        internal const int CCH_RM_SESSION_KEY = 32;

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
        /// Converts a <see cref="Process"/> to a <see cref="RM_UNIQUE_PROCESS"/>.
        /// </summary>
        /// <param name="process">The <see cref="Process"/> to convert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="process"/> is null.</exception>
        public static explicit operator RM_UNIQUE_PROCESS(Process process)
        {
            Validate.NotNull(process, nameof(process));

            var startTime = process.StartTime.ToFileTimeUtc();
            var ft = new ComTypes.FILETIME
            {
                dwHighDateTime = (int)(startTime >> 32),
                dwLowDateTime = (int)(startTime & 0xffffffff),
            };

            return new RM_UNIQUE_PROCESS
            {
                dwProcessId = process.Id,
                ProcessStartTime = ft,
            };
        }

        [MarshalAs(UnmanagedType.U4)]
        internal int dwProcessId;

        internal ComTypes.FILETIME ProcessStartTime;
    }

#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter
#pragma warning restore SA1201 // Elements must appear in the correct order
}
