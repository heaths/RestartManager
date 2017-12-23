// <copyright file="SessionManager.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using RestartManager;

    /// <summary>
    /// Manages a <see cref="RestartManagerSession"/> for the current <see cref="Runspace"/>.
    /// </summary>
    internal static class SessionManager
    {
        /// <summary>
        /// The name of the current session variable.
        /// </summary>
        public static readonly string VariableName = "RestartManagerSession";

        /// <summary>
        /// Gets a <see cref="RestartManagerSession"/> from a variable.
        /// </summary>
        /// <param name="services">Optional services to use to resolve variables.</param>
        /// <param name="variables">Variables for the current <see cref="Runspace"/>.</param>
        /// <returns>A <see cref="RestartManagerSession"/> from a variable or null if no session is available.</returns>
        public static RestartManagerSession GetVariable(IServiceProvider services, PSVariableIntrinsics variables)
        {
            var resolver = services?.GetService<IVariableService>() ?? new RunspaceVariableService(variables);
            return resolver.GetValue<RestartManagerSession>(VariableName);
        }

        /// <summary>
        /// Gets a <see cref="RestartManagerSession"/> from a parameter or from a variable.
        /// </summary>
        /// <param name="services">Optional services to use to resolve variables.</param>
        /// <param name="session">A backing field for a parameter that may reference a <see cref="RestartManagerSession"/>.</param>
        /// <param name="variables">Variables for the current <see cref="Runspace"/>.</param>
        /// <returns>A <see cref="RestartManagerSession"/> from a parameter or from a variable.</returns>
        /// <exception cref="NoSessionException">No <see cref="RestartManagerSession"/> is available.</exception>
        public static RestartManagerSession GetParameterOrVariable(IServiceProvider services, ref RestartManagerSession session, PSVariableIntrinsics variables)
        {
            if (session == null)
            {
                var resolver = services?.GetService<IVariableService>() ?? new RunspaceVariableService(variables);
                session = resolver.GetValue<RestartManagerSession>(VariableName);
            }

            return session ?? throw new NoSessionException();
        }

        /// <summary>
        /// Sets the <see cref="RestartManagerSession"/> session variable.
        /// </summary>
        /// <param name="services">Optional services to use to resolve variables.</param>
        /// <param name="variables">Variables for the current <see cref="Runspace"/>.</param>
        /// <param name="session">The <see cref="RestartManagerSession"/> to set.</param>
        public static void SetVariable(IServiceProvider services, PSVariableIntrinsics variables, RestartManagerSession session)
        {
            var resolver = services?.GetService<IVariableService>() ?? new RunspaceVariableService(variables);
            resolver.SetValue(VariableName, session);
        }
    }
}
