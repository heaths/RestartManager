// <copyright file="ActiveSessionException.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation;
    using System.Runtime.Serialization;
    using System.Threading;
    using RestartManager.Properties;

    /// <summary>
    /// Exception thrown when a <see cref="RestartManagerSession"/> session is already active within scope.
    /// </summary>
    [Serializable]
    public class ActiveSessionException : Exception, IContainsErrorRecord
    {
        private ErrorRecord error;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveSessionException"/> class.
        /// </summary>
        internal ActiveSessionException()
            : base(Resources.Error_SessionExists)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveSessionException"/> class.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> into which objects are serialized.</param>
        /// <param name="context"><see cref="StreamingContext"/> that describes the underlying stream context.</param>
        protected ActiveSessionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            error = info.GetValue(nameof(ErrorRecord), typeof(ErrorRecord)) as ErrorRecord;
        }

        /// <inheritdoc/>
        public ErrorRecord ErrorRecord => LazyInitializer.EnsureInitialized(ref error, () =>
        {
            return new ErrorRecord(this, $"RestartManager_{nameof(Resources.Error_SessionExists)}", ErrorCategory.ResourceUnavailable, null)
            {
                ErrorDetails = new ErrorDetails(Message)
                {
                    RecommendedAction = Resources.Action_StopSession,
                },
            };
        });

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ErrorRecord), ErrorRecord);
        }
    }
}
