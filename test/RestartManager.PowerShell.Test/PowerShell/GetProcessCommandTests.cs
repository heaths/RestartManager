// <copyright file="GetProcessCommandTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public class GetProcessCommandTests
    {
        [Fact]
        public void ProcessRecord_Throws()
        {
            var services = new ServiceContainer();

            var restartManagerServiceMock = new Mock<IRestartManagerService>();
            services.AddService(restartManagerServiceMock.Object);

            var sessionId = 0;
            var sessionKey = "1234";
            restartManagerServiceMock
                .Setup(x => x.StartSession(out sessionId, out sessionKey))
                .Returns(0);

            var processLength = 0;
            var processLengthRequired = 0;
            var rebootReason = RebootReason.None;
            restartManagerServiceMock
                .Setup(x => x.GetProcesses(sessionId, out processLengthRequired, ref processLength, null, out rebootReason))
                .Returns(14);

            var session = RestartManagerSession.Create(services);
            session.RegisterResources(files: new[] { @"C:\ShouldNotExist.txt" });

            var sut = new GetProcessCommand
            {
                Session = session,
            };

            var output = sut.Invoke<IProcessInfo>();
            Assert.Throws<OutOfMemoryException>(() => output.Any());
        }

        [Fact]
        public void ProcessRecord()
        {
            var services = new ServiceContainer();

            var restartManagerServiceMock = new Mock<IRestartManagerService>();
            services.AddService(restartManagerServiceMock.Object);

            var sessionId = 0;
            var sessionKey = "1234";
            restartManagerServiceMock
                .Setup(x => x.StartSession(out sessionId, out sessionKey))
                .Returns(0);

            var processLength = 0;
            var processLengthRequired = 2;
            var rebootReason = RebootReason.None;
            restartManagerServiceMock
                .Setup(x => x.GetProcesses(sessionId, out processLengthRequired, ref processLength, null, out rebootReason))
                .Returns(234);

            processLength = 2;
            restartManagerServiceMock
                .Setup(x => x.GetProcesses(sessionId, out processLengthRequired, ref processLength, It.Is<RM_PROCESS_INFO[]>(ary => ary.Length == 2), out rebootReason))
                .OutCallback((int sid, out int plr, ref int pl, RM_PROCESS_INFO[] processes, out RebootReason rr) =>
                {
                    plr = processLengthRequired;
                    rr = rebootReason;

                    processes[0] = new RM_PROCESS_INFO { ApplicationType = ApplicationType.MainWindow, bRestartable = true, strAppName = "a" };
                    processes[1] = new RM_PROCESS_INFO { ApplicationType = ApplicationType.Service, bRestartable = true, strAppName = "b", strServiceShortName = "b" };
                })
                .Returns(0);

            var session = RestartManagerSession.Create(services);
            session.RegisterResources(files: new[] { @"C:\ShouldNotExist.txt" });

            var sut = new GetProcessCommand
            {
                Session = session,
            };

            var output = sut.Invoke<IProcessInfo>();
            Assert.Collection(
                output,
                x => Assert.True(x.ApplicationType == ApplicationType.MainWindow && x.IsRestartable && x.Description == "a"),
                x => Assert.True(x.ApplicationType == ApplicationType.Service && x.IsRestartable && x.Description == "b" && x.ServiceName == "b"));
        }
    }
}
