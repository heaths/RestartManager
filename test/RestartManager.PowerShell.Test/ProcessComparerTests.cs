// <copyright file="ProcessComparerTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Xunit;

    public class ProcessComparerTests
    {
        private static readonly ProcessComparer Comparer = ProcessComparer.Default;
        private static readonly ProcessComparer StrictComparer = new ProcessComparer(StringComparer.Ordinal);

        public static IEnumerable<object[]> GetData()
        {
            var info = MockRestartManagerService.DefaultProcesses.First();
            var a = new ProcessInfo(info, RebootReason.None);

            info.ApplicationType = ApplicationType.Console;
            var b = new ProcessInfo(info, RebootReason.None);

            info.AppStatus = ApplicationStatus.Stopped;
            var c = new ProcessInfo(info, RebootReason.None);

            info.Process = new RM_UNIQUE_PROCESS { dwProcessId = info.Process.dwProcessId + 1 };
            var d = new ProcessInfo(info, RebootReason.None);

            info.strAppName = "Other";
            var e = new ProcessInfo(info, RebootReason.None);

            info.strAppName = info.strAppName.ToUpperInvariant();
            var f = new ProcessInfo(info, RebootReason.None);

            info.bRestartable = false;
            var g = new ProcessInfo(info, RebootReason.None);

            info.strServiceShortName = "Other";
            var h = new ProcessInfo(info, RebootReason.None);

            var process = Mock.Of<IProcess>(x => x.Id == 1234 && x.StartTime == new DateTime(2017, 11, 25, 17, 55, 00, 00, DateTimeKind.Local));
            info.Process = new RM_UNIQUE_PROCESS(process);
            var i = new ProcessInfo(info, RebootReason.None);

            var j = new ProcessInfo(info, RebootReason.PermissionDenied);

            var data = new[]
            {
                new object[] { null, null, true, true },
                new object[] { null, a, false, false },
                new object[] { a, a, true, true },
                new object[] { a, b, false, false },
                new object[] { b, c, false, false },
                new object[] { c, d, false, false },
                new object[] { d, e, false, false },
                new object[] { e, f, true, false },
                new object[] { f, g, false, false },
                new object[] { g, h, false, false },
                new object[] { h, i, false, false },
                new object[] { i, j, false, false },
            };

            object[] Swap(object[] input, int x, int y)
            {
                // Shallow clone is fine since elements are not changed.
                var output = (object[])input.Clone();

                var z = output[x];
                output[x] = output[y];
                output[y] = z;

                return output;
            }

            var swapped = data
                .Where(elem => !ReferenceEquals(elem[0], elem[1]))
                .Select(elem => Swap(elem, 0, 1));
            return data.Union(swapped);
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void IsEqual(IProcessInfo x, IProcessInfo y, bool equal, bool strictEqual)
        {
            Assert.Equal(equal, Comparer.Equals(x, y));
            Assert.Equal(strictEqual, StrictComparer.Equals(x, y));
        }
    }
}
