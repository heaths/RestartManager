# Contributing

Thank you for your interest in making this module better!

This is a simple solution that targets PowerShell 3.0 and newer and, therefore, .NET Framework 4.5 and newer. You should be able to build this module in Visual Studio 2012 or newer (note: developed originally in Visual Studio 2017) and all
dependencies are obtained via NuGet.

## Setup

Prior to building from the command line, Visual Studio, or Visual Studio code, make sure to run:

```
nuget restore
```

This will install everything you need.

## Style

Style is enforced through Roslyn StyleCop analyzers at development and build time, so to accept any pull requests builds must be clean. Though probably rare, additional comments on code style (for unification throughout the project) may be provided on pull requests.

## Testing

All testing is currently performed via xUnit. The CI/CD build system will ensure tests pass for Debug and Release builds, but you are encouraged to run tests via an xUnit test runner prior to pushing (or even commit to keep commits clean) such as the console or Visual Studio runners.

The CI/CD build will also publish artifacts and push NuGet prerelease packages you can use to install modules based on your changes. To enable this feature, make sure you've [installed PowerShellGet](https://docs.microsoft.com/powershell/gallery/psget/get_psget_module) 1.6 or newer and understand how [prerelease versions](https://docs.microsoft.com/en-us/powershell/gallery/psget/module/prereleasemodule) are discoverable and installable in PowerShellGet.

> *Note*: You need to install PowerShellGet into both x86 and x64 hosts because of location precedence in `PSModulePath`.

After installing (or updating), you can register the feed as a PowerShell module repository:

```powershell
install-packageprovider nuget -force # only need to do this once; may already be installed
register-psrepository -name RestartManager -sourcelocation https://ci.appveyor.com/nuget/heaths-restartmanager
```

After the repository is registered, you can find and install your module if you know your version (available on the CI/CD status page):

```powershell
find-module RestartManager -allowprerelease
install-module RestartManager -requiredversion 1.0.42-pre
```

## Questions

Questions and feedback can be submitted through [issues](https://github.com/heaths/RestartManager/issues).