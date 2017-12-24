# Windows Restart Manager Module for PowerShell

This module exposes Windows Restart Manager APIs through PowerShell cmdlets for testing applications' compatibility with Windows Restart Manager and for basic scripted installs.

[![master](https://ci.appveyor.com/api/projects/status/8hxomqwhptoip5ja/branch/master?svg=true)](https://ci.appveyor.com/project/heaths/restartmanager/branch/master)

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