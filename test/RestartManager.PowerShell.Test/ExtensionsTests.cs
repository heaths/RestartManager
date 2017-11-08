// <copyright file="ExtensionsTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE file in the project root for more information.
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
        public void GetService_Services_Null_Throws()
        {
            ITestService testService = null;

            IServiceProvider sut = null;
            Assert.Throws<ArgumentNullException>("services", () => sut.GetService(ref testService));
        }

        [Fact]
        public void GetService_Not_Defined_Throws()
        {
            ITestService testService = null;

            var sut = new ServiceContainer();
            Assert.Throws<NotImplementedException>(() => sut.GetService(ref testService, throwIfNotDefined: true));
        }

        [Fact]
        public void GetService_Defined()
        {
            var a = new TestService();
            ITestService b = null;

            var sut = new ServiceContainer();
            sut.AddService<ITestService>(a);

            var actual = sut.GetService(ref b, throwIfNotDefined: true);
            Assert.Same(a, actual);
            Assert.Same(actual, b);
        }
    }
}
