// <copyright file="ServiceContainerTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using Xunit;

    public partial class ServiceContainerTests
    {
        [Fact]
        public void AddService_Null_Throws()
        {
            var sut = new ServiceContainer();

            Assert.Throws<ArgumentNullException>("service", () => sut.AddService<object>(null));
        }

        [Fact]
        public void AddService_Defined_Throws()
        {
            var sut = new ServiceContainer();
            sut.AddService<ITestService>(new TestService());

            Assert.Throws<ArgumentException>("service", () => sut.AddService<ITestService>(new TestService()));
        }

        [Fact]
        public void AddService_Override()
        {
            ITestService a = null;
            ITestService b = null;

            var sut = new ServiceContainer();
            sut.AddService(a = new TestService());
            sut.AddService(b = new TestService(), @override: true);

            Assert.Same(b, sut.GetService<ITestService>());
        }

        [Fact]
        public void GetService_Not_Defined_Throws()
        {
            var sut = new ServiceContainer();

            Assert.Throws<NotImplementedException>(() => sut.GetService<ITestService>(throwIfNotDefined: true));
        }

        [Fact]
        public void GetService_Not_Defined()
        {
            var sut = new ServiceContainer();

            Assert.Null(sut.GetService<ITestService>());
        }

        [Fact]
        public void GetService_Defined()
        {
            var sut = new ServiceContainer();
            sut.AddService<ITestService>(new TestService());

            Assert.IsAssignableFrom<ITestService>(sut.GetService<ITestService>());
        }
    }
}
