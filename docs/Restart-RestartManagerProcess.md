---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Restart-RestartManagerProcess
schema: 2.0.0
---

# Restart-RestartManagerProcess

## SYNOPSIS
Attempts to restart any processes previously shut down by the Restart Manager.

## SYNTAX

```
Restart-RestartManagerProcess [-Session <RestartManagerSession>]
```

## DESCRIPTION
Attempts to restart any processes previously shut down by the Restart Manager for the active session in $RestartManagerSession or passed to -Session. No all processes may restart.

## EXAMPLES

### Example 1
```
PS C:\> Restart-RestartManagerProcess
```

Attempts to restart any processes previously shut down by the Restart Manager.

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

## RELATED LINKS
[Get-RestartManagerProcess](Get-RestartManagerProcess)
[Stop-RestartManagerProcess](Restart-RestartManagerProcess)
[RmRestart](https://msdn.microsoft.com/library/windows/desktop/aa373665.aspx)
[RegisterApplicationRestart](https://msdn.microsoft.com/library/windows/desktop/aa373347.aspx)