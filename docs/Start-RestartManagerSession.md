---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Start-RestartManagerSession
schema: 2.0.0
---

# Start-RestartManagerSession

## SYNOPSIS
Starts a new Restart Manager session.

## SYNTAX

```
Start-RestartManagerSession [-PassThru] [-Force]
```

## DESCRIPTION
Starts a new Restart Manager session and sets the $RestartManagerSession variable so you do not need to pass the session to each subsequent cmdlet. If there is already a session active this cmdlet will fail unless you pass -Force, in which case the active session is disposed and a new one created in its place.

## EXAMPLES

### Example 1
```
PS C:\> Start-RestartManagerSession
```

Starts a new Restart Manager session.

## PARAMETERS

### -Force
Creates a new session in $RestartManagerSession even if one is already active. The existing session is disposed.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
Writes the Restart Manager session to the pipeline.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## OUTPUTS

### RestartManager.RestartManagerSession

## RELATED LINKS
[Stop-RestartManagerSession](Stop-RestartManagerSession)
[RmStartSession](https://msdn.microsoft.com/library/windows/desktop/aa373668.aspx)