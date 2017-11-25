// <copyright file="ExtensionsTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ExtensionsTests
    {
        [Fact]
        public void NullOrEmpty_Null()
        {
            IEnumerable<string> sut = null;
            Assert.True(sut.NullOrEmpty());
        }

        [Fact]
        public void NullOrEmpty_Empty()
        {
            Assert.True(string.Empty.NullOrEmpty());
        }

        [Fact]
        public void NullOrEmpty_Not_Empty()
        {
            Assert.False("test".NullOrEmpty());
        }

        [Fact]
        public void GetService_Services_Null_Creates()
        {
            ITestService testService = null;

            IServiceProvider sut = null;
            var actual = sut.GetService(ref testService, () => new TestService());

            Assert.NotNull(testService);
            Assert.Same(testService, actual);
        }

        [Fact]
        public void GetService_Null_Throws()
        {
            ITestService testService = null;

            var sut = new ServiceContainer();
            Assert.Throws<ArgumentNullException>("factory", () => sut.GetService(ref testService, null));
        }

        [Fact]
        public void GetService_Not_Defined_Creates()
        {
            ITestService testService = null;

            var sut = new ServiceContainer();
            var actual = sut.GetService(ref testService, () => new TestService());

            Assert.NotNull(testService);
            Assert.Same(testService, actual);
        }

        [Fact]
        public void GetService_Defined()
        {
            var a = new TestService();
            ITestService b = null;

            var sut = new ServiceContainer();
            sut.AddService<ITestService>(a);

            var actual = sut.GetService(ref b, () => throw new Exception());
            Assert.Same(a, actual);
            Assert.Same(actual, b);
        }
    }
}
