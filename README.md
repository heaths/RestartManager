# Windows Restart Manager Module for PowerShell

This module exposes Windows Restart Manager APIs through PowerShell cmdlets for testing applications' compatibility with Windows Restart Manager and for basic scripted installs.

[![Build Status](https://dev.azure.com/heaths/public/_apis/build/status/RestartManager-CI?branchName=develop)](https://dev.azure.com/heaths/public/_build/latest?definitionId=15)
[![CodeCov](https://codecov.io/gh/heaths/RestartManager/branch/master/graph/badge.svg)](https://codecov.io/gh/heaths/RestartManager)
[![GitHub Release](https://img.shields.io/github/release/heaths/RestartManager.svg)](https://github.com/heaths/RestartManager/releases/latest)
[![PSGallery](https://img.shields.io/powershellgallery/dt/RestartManager.svg)](https://www.powershellgallery.com/packages/RestartManager)

## Examples

You can use this module to, for example, replace files that may be in use by other processes.

```powershell
Start-RestartManagerProcess
dir .\MyApp -Filter *.dll -Recurse | Register-RestartManagerResource
Stop-RestartManagerProcess
Expand-Archive .\MyApp.zip .\MyApp -Force
Restart-RestartManagerProcess
Stop-RestartManagerProcess
```

You can also use this module to just figure out what processes may be using certain files.

```powershell
Start-RestartManagerProcess
dir .\MyApp -Filter *.dll -Recurse | Register-RestartManagerResource
Get-RestartManagerProcess
Stop-RestartManagerProcess
```

For more examples and help content, please see the [wiki](https://github.com/heaths/RestartManager/wiki).

## Feedback

To file issues or suggestions, please use the [Issues](https://github.com/heaths/RestartManager/issues) page for this project on GitHub.

## License

This project is licensed under the [MIT license](LICENSE.txt).