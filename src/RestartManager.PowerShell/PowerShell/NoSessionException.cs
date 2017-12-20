// <copyright file="NoSessionException.cs" company="Heath Stewart">
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
    /// Exception thrown when no <see cref="RestartManagerSession"/> is defined within scope.
    /// </summary>
    [Serializable]
    public class NoSessionException : Exception, IContainsErrorRecord
    {
        private ErrorRecord error;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSessionException"/> class.
        /// </summary>
        internal NoSessionException()
            : base(Resources.Error_NoSession)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSessionException"/> class.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> into which objects are serialized.</param>
        /// <param name="context"><see cref="StreamingContext"/> that describes the underlying stream context.</param>
        protected NoSessionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            error = info.GetValue(nameof(ErrorRecord), typeof(ErrorRecord)) as ErrorRecord;
        }

        /// <inheritdoc/>
        public ErrorRecord ErrorRecord => LazyInitializer.EnsureInitialized(ref error, () =>
        {
            return new ErrorRecord(this, $"RestartManager_{nameof(Resources.Error_NoSession)}", ErrorCategory.ResourceExists, null)
            {
                ErrorDetails = new ErrorDetails(Message)
                {
                    RecommendedAction = Resources.Action_StartSession,
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
