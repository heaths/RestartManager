// <copyright file="ValidateTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using RestartManager.Properties;
    using Xunit;

    public class ValidateTests
    {
        [Fact]
        public void NotEmpty_Null()
        {
            Validate.NotEmpty(null, "param");
        }

        [Fact]
        public void NotEmpty_Throws()
        {
            Assert.Throws<ArgumentException>("param", () => Validate.NotEmpty(string.Empty, "param"));
        }

        [Fact]
        public void NotEmpty()
        {
            Validate.NotEmpty("value", "param");
        }

        [Fact]
        public void NotNull_Throws()
        {
            Assert.Throws<ArgumentNullException>("param", () => Validate.NotNull<object>(null, "param"));
        }

        [Fact]
        public void NotNull()
        {
            Validate.NotNull(string.Empty, "param");
        }

        [Fact]
        public void NotNullOrEmpty_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>("param", () => Validate.NotNullOrEmpty(null, "param"));
        }

        [Fact]
        public void NotNullOrEmpty_Empty_Throws()
        {
            Assert.Throws<ArgumentException>("param", () => Validate.NotNullOrEmpty(string.Empty, "param"));
        }

        [Fact]
        public void NotNullOrEmpty()
        {
            Validate.NotNullOrEmpty("value", "param");
        }
    }
}
