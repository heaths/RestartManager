---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Stop-RestartManagerSession
schema: 2.0.0
---

# Stop-RestartManagerSession

## SYNOPSIS
Stops the active Restart Manager Session.

## SYNTAX

```
Stop-RestartManagerSession [-Session <RestartManagerSession>]
```

## DESCRIPTION
Stops the active Restart Manager session in $RestartManagerSession or that is passed to -Session. Will not err if no session is active or the session has already been disposed.

## EXAMPLES

### Example 1
```
PS C:\> Stop-RestartManagerSession
```

Stops the active Restart Manager session.

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
[Start-RestartManagerSession](Start-RestartManagerSession)
[RmEndSession](https://msdn.microsoft.com/library/windows/desktop/aa373659.aspx)