// <copyright file="RestartManagerSessionTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using Moq;
    using Xunit;

    public class RestartManagerSessionTests
    {
        [Theory]
        [InlineData(NativeMethods.ERROR_OUTOFMEMORY, typeof(OutOfMemoryException))]
        [InlineData(NativeMethods.ERROR_BAD_ARGUMENTS, typeof(ArgumentException))]
        [InlineData(NativeMethods.ERROR_MAX_SESSIONS_REACHED, typeof(Win32Exception))]
        public void Create_Throws(int error, Type exceptionType)
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(error);

            Assert.Throws(exceptionType, () => RestartManagerSession.Create(services));
        }

        [Fact]
        public void Create_Starts_Session()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            using (var sut = RestartManagerSession.Create(services))
            {
                Assert.Equal(1, sut.SessionId);
                Assert.Equal("1234abcd", sut.SessionKey);
            }
        }

        [Fact]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Testing that double dispose does not throw")]
        public void Dispose_Ends_Session()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            using (var sut = RestartManagerSession.Create(services))
            {
                // Make sure we do not attempt to end again.
                sut.Dispose();
                Assert.True(sut.IsDisposed);
            }

            rmMock.Verify(x => x.EndSession(1), Times.Once);
        }

        [Fact]
        public void RegisterResources_Disposed_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            RestartManagerSession sut;
            using (sut = RestartManagerSession.Create(services))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => sut.RegisterResources(files: new[] { @"C:\ShouldNotExist" }));
        }

        [Fact]
        public void RegisterResources_No_Resources()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.RegisterResources();
            }

            rmMock.Verify(x => x.RegisterResources(It.IsAny<int>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public void RegisterResources_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.RegisterResources(sessionId, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>())).Returns(NativeMethods.ERROR_OUTOFMEMORY);

            using (var sut = RestartManagerSession.Create(services))
            {
                Assert.Throws<OutOfMemoryException>(() => sut.RegisterResources(files: new[] { @"C:\ShouldNotExist" }));
            }
        }

        [Fact]
        public void RegisterResources()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.RegisterResources(sessionId, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>())).Returns(NativeMethods.ERROR_SUCCESS);

            var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                @"C:\ShouldNotExist",
            };

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.RegisterResources(files: files);
            }

            rmMock.Verify(x => x.RegisterResources(1, It.Is<IEnumerable<string>>(y => files.SetEquals(y)), null, null), Times.Once);
        }

        [Fact]
        public void ShutdownProcesses_Disposed_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            RestartManagerSession sut;
            using (sut = RestartManagerSession.Create(services))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => sut.ShutdownProcesses());
        }

        [Fact]
        public void ShutdownProcesses_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.ShutdownProcesses(sessionId, RM_SHUTDOWN_TYPE.RmForceShutdown, It.IsAny<RM_WRITE_STATUS_CALLBACK>())).Returns(NativeMethods.ERROR_OUTOFMEMORY);

            using (var sut = RestartManagerSession.Create(services))
            {
                Assert.Throws<OutOfMemoryException>(() => sut.ShutdownProcesses(force: true));
            }
        }

        [Fact]
        public void ShutdownProcesses()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.ShutdownProcesses(sessionId, 0, It.IsAny<RM_WRITE_STATUS_CALLBACK>())).Returns(NativeMethods.ERROR_SUCCESS).Verifiable();

            var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                @"C:\ShouldNotExist",
            };

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.ShutdownProcesses();
            }

            rmMock.Verify();
        }

        [Fact]
        public void RestartProcesses_Disposed_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            RestartManagerSession sut;
            using (sut = RestartManagerSession.Create(services))
            {
            }

            Assert.Throws<ObjectDisposedException>(() => sut.RestartProcesses());
        }

        [Fact]
        public void RestartProcesses_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.RestartProcesses(sessionId, It.IsAny<RM_WRITE_STATUS_CALLBACK>())).Returns(NativeMethods.ERROR_OUTOFMEMORY);

            using (var sut = RestartManagerSession.Create(services))
            {
                Assert.Throws<OutOfMemoryException>(() => sut.RestartProcesses());
            }
        }

        [Fact]
        public void RestartProcesses()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.RestartProcesses(sessionId, It.IsAny<RM_WRITE_STATUS_CALLBACK>())).Returns(NativeMethods.ERROR_SUCCESS).Verifiable();

            var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                @"C:\ShouldNotExist",
            };

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.RestartProcesses();
            }

            rmMock.Verify();
        }
    }
}
