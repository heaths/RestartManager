# RestartManager
## about_RestartManager

# SHORT DESCRIPTION
Windows Restart Manager PowerShell Module

# LONG DESCRIPTION
This is a PowerShell module that provides access to the Windows Restart Manager. You can use it in scripts to help avoid reboots when copying files that may be used by other programs, or to test your own products and installers for how they work with the Restart Manager.

# EXAMPLES
You can register files, processes, and services to see what processes are using them and, optionally, stop and restart them:

```powershell
Start-RestartManagerSession
Get-ChildItem .\MyApp -Filter *.dll -Recurse | Register-RestartManagerResource
Get-RestartManagerProcess
Stop-RestartManagerSession
```

If you want to stop and restart processes, before calling `Stop-RestartManagerSession` above call the following cmdlets:

```powershell
Stop-RestartManagerProcess
# Perform other operations here, like copy over existing files.
Restart-RestartManagerProcess
```

You can find the session ID and key for the current Restart Manager session in the `$RestartManagerSession` property.

# SEE ALSO
- https://github.com/heaths/RestartManager
- https://msdn.microsoft.com/library/windows/desktop/cc948910.aspx

# KEYWORDS
- RM
- Restart Manager
