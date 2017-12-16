// <copyright file="TestExtensions.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System.Reflection;
    using Moq.Language;
    using Moq.Language.Flow;

    /// <summary>
    /// Extension methods for tests.
    /// </summary>
    public static class TestExtensions
    {
        #region OutCallback
        // From https://stackoverflow.com/questions/1068095/assigning-out-ref-parameters-in-moq/19598345#19598345

        public delegate void OutAction<T1, T2, T3, T4, T5>(T1 arg1, out T2 arg2, ref T3 arg3, T4 arg4, out T5 arg5);

        public static IReturnsThrows<TMock, TReturn> OutCallback<TMock, TReturn, T1, T2, T3, T4, T5>(this ICallback<TMock, TReturn> mock, OutAction<T1, T2, T3, T4, T5> action)
            where TMock : class
        {
            return OutCallbackInternal(mock, action);
        }

        private static IReturnsThrows<TMock, TReturn> OutCallbackInternal<TMock, TReturn>(ICallback<TMock, TReturn> mock, object action)
            where TMock : class
        {
            mock.GetType()
                .Assembly.GetType("Moq.MethodCall")
                .InvokeMember("SetCallbackWithArguments", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, mock, new[] { action });

            return mock as IReturnsThrows<TMock, TReturn>;
        }
        #endregion
    }
}
