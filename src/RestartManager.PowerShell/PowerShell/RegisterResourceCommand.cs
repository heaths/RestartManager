// <copyright file="RegisterResourceCommand.cs" company="Heath Stewart">
// Copyright (c) 2017 Heath Stewart
// See the LICENSE.txt file in the project root for more information.
// </copyright>

namespace RestartManager.PowerShell
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using RestartManager.Properties;

    /// <summary>
    /// The Register-RestartManagerResource cmdlet.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, Nouns.RestartManagerResource, DefaultParameterSetName = nameof(Path))]
    public class RegisterResourceCommand : PSCmdlet
    {
        private readonly Lazy<IList<string>> files = new Lazy<IList<string>>(() => new List<string>());
        private readonly Lazy<IList<string>> services = new Lazy<IList<string>>(() => new List<string>());
        private readonly Lazy<IList<IProcess>> processes = new Lazy<IList<IProcess>>(() => new List<IProcess>());

        /// <summary>
        /// Gets or sets the path of a file resource to add. Wildcards are supported.
        /// </summary>
        [Parameter(ParameterSetName = nameof(Path), Position = 0)]
        [ValidateNotNullOrEmpty]
        public string[] Path { get; set; }

        /// <summary>
        /// Gets or sets the path of a file resource to add. Wildcards are not supported.
        /// </summary>
        [Alias("PSPath")]
        [Parameter(ParameterSetName = nameof(LiteralPath), ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string[] LiteralPath
        {
            get => Path;
            set => Path = value;
        }

        /// <summary>
        /// Gets or sets the name of a service to add.
        /// </summary>
        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string[] ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Process"/> to add.
        /// </summary>
        [Parameter(ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        public Process[] Process { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RestartManagerSession"/> with which to register resources.
        /// </summary>
        [Parameter(Mandatory = true)]
        public RestartManagerSession Session { get; set; }

        /// <inheritdoc/>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (Path != null)
            {
                var paths = SessionState.InvokeProvider.Item
                    .Get(Path, true, nameof(LiteralPath).Equals(ParameterSetName, StringComparison.OrdinalIgnoreCase))
                    .Where(obj => obj.BaseObject is FileInfo)
                    .Select(obj => obj.BaseObject as FileInfo)
                    .Select(info => info.FullName);

                if (paths.NullOrEmpty())
                {
                    WriteWarning(Resources.Warning_NoFiles);
                }
                else
                {
                    foreach (var path in paths)
                    {
                        files.Value.Add(path);
                    }
                }
            }

            if (ServiceName != null)
            {
                foreach (var service in ServiceName)
                {
                    services.Value.Add(service);
                }
            }

            if (Process != null)
            {
                foreach (var process in Process)
                {
                    var adapter = new ProcessAdapter(process);
                    processes.Value.Add(adapter);
                }
            }
        }

        /// <inheritdoc/>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            Session.RegisterResources(
                files: files.IsValueCreated ? files.Value : null,
                processes: processes.IsValueCreated ? processes.Value : null,
                services: services.IsValueCreated ? services.Value : null);
        }
    }
}
