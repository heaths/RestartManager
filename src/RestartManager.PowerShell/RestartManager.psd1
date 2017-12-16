# Copyright (c) 2017 Heath Stewart
# See the LICENSE.txt file in the project root for more information.

@{
GUID = '695441F7-3563-4BB3-B465-FAF9F7E489F1'
Author = 'Heath Stewart'
CompanyName = 'Heath Stewart'
Description = 'Exposes Windows Restart Manager to Windows PowerShell'
Copyright = 'Copyright (c) 2017 Heath Stewart'
ModuleVersion = '1.0.0.0'
PowerShellVersion = '3.0'
ModuleToProcess = 'RestartManager.PowerShell.dll'
CmdletsToExport = @(
  # Explicitly export cmdlets to support constrained language mode.
  'Get-RestartManagerProcess'
  'Register-RestartManagerResource'
  'Restart-RestartManagerProcess'
  'Start-RestartManagerSession'
  'Stop-RestartManagerProcess'
  'Stop-RestartManagerSession'
)
PrivateData = @{
  PSData = @{
    ProjectUri = 'https://github.com/heaths/restartmanager'
    LicenseUri = 'https://raw.githubusercontent.com/heaths/restartmanager/master/LICENSE.txt'
  }
}
}
