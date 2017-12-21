// <copyright file="ProcessComparerTests.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            var g = new ProcessInfo(info, RebootReason.PermissionDenied);

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
            };

            object[] Swap(object[] obj, int x, int y)
            {
                var z = obj[x];
                obj[x] = obj[y];
                obj[y] = z;

                return obj;
            }

            return data
                .Union(data.Select(elem => Swap(elem, 0, 1)));
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
