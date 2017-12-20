// <copyright file="RunspaceCollection.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using Xunit;

    /// <summary>
    /// A test collection for PowerShell cmdlets and functions.
    /// </summary>
    [CollectionDefinition(DefinitionName)]
    public sealed class RunspaceCollection : ICollectionFixture<RunspaceFixture>
    {
        /// <summary>
        /// The name of the test collection definition.
        /// </summary>
        internal const string DefinitionName = nameof(RunspaceCollection);
    }
}
