// <copyright file="RunspaceException.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Management.Automation;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception throw when an <see cref="ErrorRecord"/> is written to the errors stream.
    /// </summary>
    [Serializable]
    public class RunspaceException : Exception, IContainsErrorRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunspaceException"/> class.
        /// </summary>
        /// <param name="error">The <see cref="ErrorRecord"/> that was written to the stream.</param>
        internal RunspaceException(ErrorRecord error)
            : base(error.ToString(), error.Exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RunspaceException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> into which the <see cref="ErrorRecord"/> is written.</param>
        /// <param name="context"><see cref="StreamingContext"/> when serializing the exception.</param>
        protected RunspaceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorRecord = info.GetValue(nameof(ErrorRecord), typeof(ErrorRecord)) as ErrorRecord;
        }

        /// <inheritdoc/>
        public ErrorRecord ErrorRecord { get; }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ErrorRecord), ErrorRecord);
        }
    }
}
