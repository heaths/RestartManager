// <copyright file="RestartManagerSessionTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Moq;
    using Xunit;

    public class RestartManagerSessionTests
    {
        [Fact]
        public void Create_Services_Null()
        {
            Assert.Throws<ArgumentNullException>("services", () => RestartManagerSession.Create(null));
        }

        [Fact]
        public void Create_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_OUTOFMEMORY);

            Assert.Throws<OutOfMemoryException>(() => RestartManagerSession.Create(services));
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
        public void Register_Disposed_Throws()
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

            Assert.Throws<ObjectDisposedException>(() => sut.Register(files: new[] { @"C:\ShouldNotExist" }));
        }

        [Fact]
        public void Register_No_Resources()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.Register();
            }

            rmMock.Verify(x => x.Register(It.IsAny<int>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public void Register_Throws()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.Register(It.IsAny<int>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>())).Returns(NativeMethods.ERROR_OUTOFMEMORY);

            using (var sut = RestartManagerSession.Create(services))
            {
                Assert.Throws<OutOfMemoryException>(() => sut.Register(files: new[] { @"C:\ShouldNotExist" }));
            }
        }

        [Fact]
        public void Register()
        {
            var services = new ServiceContainer();

            var rmMock = new Mock<IRestartManagerService>();
            services.AddService(rmMock.Object);

            var sessionId = 1;
            var sessionKey = "1234abcd";
            rmMock.Setup(x => x.StartSession(out sessionId, out sessionKey)).Returns(NativeMethods.ERROR_SUCCESS);
            rmMock.Setup(x => x.Register(It.IsAny<int>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<RM_UNIQUE_PROCESS>>(), It.IsAny<IEnumerable<string>>())).Returns(NativeMethods.ERROR_SUCCESS);

            var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                @"C:\ShouldNotExist",
            };

            using (var sut = RestartManagerSession.Create(services))
            {
                sut.Register(files: files);
            }

            rmMock.Verify(x => x.Register(1, It.Is<IEnumerable<string>>(y => files.SetEquals(y)), null, null), Times.Once);
        }
    }
}
