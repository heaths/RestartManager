---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Get-RestartManagerProcess
schema: 2.0.0
---

# Get-RestartManagerProcess

## SYNOPSIS
Gets information about processes using any previously registered resource.

## SYNTAX

```
Get-RestartManagerProcess [-Session <RestartManagerSession>]
```

## DESCRIPTION
Gets information about processes using any previous registered resource for the active session in $RestartManagerSession or passed to -Session.

## EXAMPLES

### Example 1
```
PS C:\> Get-RestartManagerProcess
```

Gets information about processes using any previously registered resource.

## PARAMETERS

### -Session
The Restart Manager session to use. If not specified, the $RestartManagerSession variable is used.

```yaml
Type: RestartManagerSession
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## OUTPUTS

### RestartManager.ProcessInfo

## RELATED LINKS
[Register-RestartManagerResource](Register-RestartManagerResource)
[RmGetList](https://msdn.microsoft.com/library/windows/desktop/aa373661.aspx)