// <copyright file="ValidateTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using Xunit;

    public class ValidateTests
    {
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
    }
}
