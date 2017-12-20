// <copyright file="RunspaceVariableService.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    /// <summary>
    /// Gets variables from the given <see cref="Runspace"/>.
    /// </summary>
    internal class RunspaceVariableService : IVariableService
    {
        private readonly PSVariableIntrinsics variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunspaceVariableService"/> class.
        /// </summary>
        /// <param name="variables">The <see cref="PSVariableIntrinsics"/> to wrap.</param>
        public RunspaceVariableService(PSVariableIntrinsics variables)
        {
            this.variables = variables;
        }

        /// <inheritdoc/>
        public T GetValue<T>(string name)
            where T : class
        {
            Validate.NotNullOrEmpty(name, nameof(name));

            return variables?.GetValue(name) as T;
        }

        /// <inheritdoc/>
        public void SetValue<T>(string name, T value)
            where T : class
        {
            Validate.NotNullOrEmpty(name, nameof(name));

            variables?.Set(name, value);
        }
    }
}
