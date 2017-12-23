// <copyright file="RunspaceVariableServiceTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using Xunit;

    public class RunspaceVariableServiceTests : IClassFixture<RunspaceFixture>
    {
        private readonly RunspaceFixture fixture;

        public RunspaceVariableServiceTests(RunspaceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void New_Null()
        {
            var sut = new RunspaceVariableService(null);
            sut.SetValue<string>(nameof(New_Null), null);

            Assert.Null(sut.GetValue<string>(nameof(New_Null)));
        }

        [Fact]
        public void Gets_Sets_Same_Type()
        {
            const string expected = nameof(expected);

            var sut = new RunspaceVariableService(fixture.Variables);

            sut.SetValue(nameof(Gets_Sets_Same_Type), expected);
            var actual = sut.GetValue<string>(nameof(Gets_Sets_Same_Type));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Gets_Sets_Different_Types()
        {
            var sut = new RunspaceVariableService(fixture.Variables);

            sut.SetValue(nameof(Gets_Sets_Different_Types), (object)0);
            var actual = sut.GetValue<string>(nameof(Gets_Sets_Different_Types));

            Assert.Null(actual);
        }
    }
}
