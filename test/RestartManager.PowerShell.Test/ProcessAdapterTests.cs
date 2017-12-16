// <copyright file="ProcessAdapterTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Diagnostics;
    using Xunit;

    public class ProcessAdapterTests
    {
        [Fact]
        public void New_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>("process", () => new ProcessAdapter(null));
        }

        [Fact]
        public void New()
        {
            var current = Process.GetCurrentProcess();

            var sut = new ProcessAdapter(current);

            Assert.Equal(current.Id, sut.Id);
            Assert.Equal(current.StartTime, sut.StartTime.DateTime);
        }
    }
}
