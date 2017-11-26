// <copyright file="RestartManagerSession.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// A Windows Restart Manager session.
    /// </summary>
    public class RestartManagerSession : IDisposable
    {
        private readonly IServiceProvider services;
        private IRestartManagerService restartManagerService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestartManagerSession"/> class.
        /// </summary>
        /// <param name="services">Services to provide this object.</param>
        /// <param name="sessionId">The session identity.</param>
        /// <param name="sessionKey">The session key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
        private RestartManagerSession(IServiceProvider services, int sessionId, string sessionKey)
        {
            this.services = services;

            SessionId = sessionId;
            SessionKey = sessionKey;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RestartManagerSession"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~RestartManagerSession()
        {
            Dispose(false);
        }

        /// <summary>
        /// Progress reported when shutting down applications and services.
        /// </summary>
        /// <seealso cref="ShutdownProcesses(bool, bool)"/>
        internal event EventHandler<ProgressEventArgs> ShutdownProgress;

        /// <summary>
        /// Progress reported when restarting applications and services.
        /// </summary>
        /// <seealso cref="RestartProcesses"/>
        internal event EventHandler<ProgressEventArgs> RestartProgress;

        /// <summary>
        /// Gets the session identity.
        /// </summary>
        public int SessionId { get; }

        /// <summary>
        /// Gets the session key.
        /// </summary>
        public string SessionKey { get; }

        /// <summary>
        /// Gets a value indicating whether this object is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether any processes are registered.
        /// </summary>
        public bool IsRegistered { get; private set; }

        private IRestartManagerService RestartManagerService =>
            services.GetService(ref restartManagerService, () => WindowsRestartManagerService.Default);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates and starts a new <see cref="RestartManagerSession"/>.
        /// </summary>
        /// <param name="services">Services to create and start a new <see cref="RestartManagerSession"/>.</param>
        /// <returns>A new <see cref="RestartManagerSession"/>.</returns>
        internal static RestartManagerSession Create(IServiceProvider services)
        {
            var restartManagerService = services?.GetService<IRestartManagerService>() ?? WindowsRestartManagerService.Default;
            var error = restartManagerService.StartSession(out var sessionId, out var sessionKey);
            ThrowOnError(error);

            return new RestartManagerSession(services, sessionId, sessionKey);
        }

        /// <summary>
        /// Throws a <see cref="Win32Exception"/> if <paramref name="error"/> is an error.
        /// </summary>
        /// <param name="error">A Win32 error code.</param>
        /// <exception cref="Win32Exception"><paramref name="error"/> is an error.</exception>
        internal static void ThrowOnError(int error)
        {
            if (error == NativeMethods.ERROR_OUTOFMEMORY)
            {
                throw new OutOfMemoryException();
            }
            else if (error == NativeMethods.ERROR_BAD_ARGUMENTS)
            {
                throw new ArgumentException();
            }
            else if (error != NativeMethods.ERROR_SUCCESS)
            {
                throw new Win32Exception(error);
            }
        }

        /// <summary>
        /// Registers resources with the Restart Manager.
        /// </summary>
        /// <param name="files">Optional collection of file paths.</param>
        /// <param name="processes">Optional collection of processes.</param>
        /// <param name="services">Optional collection of service names.</param>
        /// <exception cref="ObjectDisposedException">This object has already been disposed.</exception>
        /// <exception cref="Win32Exception">An error occured.</exception>
        internal void RegisterResources(IEnumerable<string> files = null, IEnumerable<Process> processes = null, IEnumerable<string> services = null)
        {
            ThrowIfDisposed();

            if (!files.NullOrEmpty() || !processes.NullOrEmpty() || !services.NullOrEmpty())
            {
                IsRegistered = true;

                var uniqueProcesses = processes?.Select(process => (RM_UNIQUE_PROCESS)process);
                var error = RestartManagerService.RegisterResources(SessionId, files, uniqueProcesses, services);
                ThrowOnError(error);
            }
        }

        /// <summary>
        /// Shuts down applications and services identity by the Restart Manager for registered resources.
        /// </summary>
        /// <param name="force">Whether to force applications to shutdown if no response. The default is false.</param>
        /// <param name="onlyRegistered">Whether to only shut down applications if registered with Restart Manager. The default is false.</param>
        /// <exception cref="ObjectDisposedException">This object has already been disposed.</exception>
        /// <exception cref="Win32Exception">An error occured.</exception>
        /// <seealso cref="ShutdownProgress"/>
        internal void ShutdownProcesses(bool force = false, bool onlyRegistered = false)
        {
            ThrowIfDisposed();

            RM_SHUTDOWN_TYPE shutdownType = 0;
            if (force)
            {
                shutdownType |= RM_SHUTDOWN_TYPE.RmForceShutdown;
            }

            if (onlyRegistered)
            {
                shutdownType |= RM_SHUTDOWN_TYPE.RmShutdownOnlyRegistered;
            }

            var error = RestartManagerService.ShutdownProcesses(SessionId, shutdownType, OnShutdownProgress);
            ThrowOnError(error);
        }

        /// <summary>
        /// Restarts applications and services previously shut down by Restart Manager.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This object has already been disposed.</exception>
        /// <exception cref="Win32Exception">An error occured.</exception>
        /// <seealso cref="RestartProgress"/>
        internal void RestartProcesses()
        {
            ThrowIfDisposed();

            var error = RestartManagerService.RestartProcesses(SessionId, OnRestartProgress);
            ThrowOnError(error);
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if this object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException"><see cref="IsDisposed"/> is true.</exception>
        internal void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(RestartManagerSession));
            }
        }

        private void OnShutdownProgress(int percentComplete) =>
            ShutdownProgress?.Invoke(this, new ProgressEventArgs(percentComplete));

        private void OnRestartProgress(int percentComplete) =>
            RestartProgress?.Invoke(this, new ProgressEventArgs(percentComplete));

        private void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            RestartManagerService.EndSession(SessionId);
            IsDisposed = true;
        }
    }
}
