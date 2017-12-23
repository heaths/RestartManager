// <copyright file="ProcessInfoTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using Xunit;

    public class ProcessInfoTests
    {
        [Fact]
        public void New_Converts()
        {
            var info = new RM_PROCESS_INFO
            {
                ApplicationType = ApplicationType.MainWindow,
                AppStatus = ApplicationStatus.Running,
                bRestartable = true,
                Process = new RM_UNIQUE_PROCESS
                {
                    dwProcessId = 1234,
                    ProcessStartTime = new System.Runtime.InteropServices.ComTypes.FILETIME
                    {
                        dwHighDateTime = 30635685,
                        dwLowDateTime = 1855111194,
                    },
                },
                strAppName = "TestApp",
                strServiceShortName = "TestService",
                TSSessionId = 1,
            };

            var sut = new ProcessInfo(info, RebootReason.PermissionDenied);

            Assert.Equal(ApplicationStatus.Running, sut.ApplicationStatus);
            Assert.Equal(ApplicationType.MainWindow, sut.ApplicationType);
            Assert.Equal("TestApp", sut.Description);
            Assert.Equal(1234, sut.Id);
            Assert.True(sut.IsRestartable);
            Assert.Equal(RebootReason.PermissionDenied, sut.RebootReason);
            Assert.Equal("TestService", sut.ServiceName);
            Assert.Equal(DateTimeOffset.FromFileTime(131579267020668954), sut.StartTime);
        }

        [Fact]
        public void IsEqual()
        {
            var info = new RM_PROCESS_INFO
            {
                Process = new RM_UNIQUE_PROCESS
                {
                    dwProcessId = 0,
                },
                ApplicationType = ApplicationType.MainWindow,
                bRestartable = true,
            };

            var a = new ProcessInfo(info, RebootReason.None);

            info.ApplicationType = ApplicationType.Console;
            info.bRestartable = false;

            var b = new ProcessInfo(info, RebootReason.PermissionDenied);

            Assert.True(a.Equals(a));
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
        }
    }
}
