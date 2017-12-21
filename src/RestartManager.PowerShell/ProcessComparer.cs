// <copyright file="ProcessComparer.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Compares <see cref="IProcessInfo"/> objects.
    /// </summary>
    public class ProcessComparer : IEqualityComparer<IProcessInfo>
    {
        /// <summary>
        /// Gets the default <see cref="ProcessComparer"/>.
        /// </summary>
        public static readonly ProcessComparer Default = new ProcessComparer();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessComparer"/> class.
        /// </summary>
        /// <param name="stringComparer">The <see cref="StringComparer"/> to use for string comparisons. The default is <see cref="StringComparer.OrdinalIgnoreCase"/>.</param>
        public ProcessComparer(StringComparer stringComparer = null)
        {
            StringComparer = stringComparer ?? StringComparer.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Gets the <see cref="StringComparer"/> used for string comparisons.
        /// </summary>
        public StringComparer StringComparer { get; }

        /// <inheritdoc/>
        public bool Equals(IProcessInfo x, IProcessInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x is null)
            {
                return false;
            }
            else if (y is null)
            {
                return false;
            }

            if (x.ApplicationStatus != y.ApplicationStatus)
            {
                return false;
            }

            if (x.ApplicationType != y.ApplicationType)
            {
                return false;
            }

            if (!StringComparer.Equals(x.Description, y.Description))
            {
                return false;
            }

            if (x.Id != y.Id)
            {
                return false;
            }

            if (x.IsRestartable != y.IsRestartable)
            {
                return false;
            }

            if (x.RebootReason != y.RebootReason)
            {
                return false;
            }

            if (!StringComparer.Equals(x.ServiceName, y.ServiceName))
            {
                return false;
            }

            if (x.StartTime != y.StartTime)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public int GetHashCode(IProcessInfo obj)
        {
            if (obj is null)
            {
                return 0;
            }

            return obj.ApplicationStatus.GetHashCode()
                ^ obj.ApplicationType.GetHashCode()
                ^ StringComparer.GetHashCode(obj.Description)
                ^ obj.Id.GetHashCode()
                ^ obj.IsRestartable.GetHashCode()
                ^ obj.RebootReason.GetHashCode()
                ^ StringComparer.GetHashCode(obj.ServiceName)
                ^ obj.StartTime.GetHashCode();
        }
    }
}
