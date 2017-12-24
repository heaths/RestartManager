---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Stop-RestartManagerProcess
schema: 2.0.0
---

# Stop-RestartManagerProcess

## SYNOPSIS
Stops processes using any previously registered resources.

## SYNTAX

```
Stop-RestartManagerProcess [-Force] [-OnlyRegistered] [-Session <RestartManagerSession>]
```

## DESCRIPTION
Stops processes using any previously registered resources for the active session in $RestartManagerSession or passed to -Session. You can forcibly stop processes after a system-defined timeout by passing the -Force switch.

## EXAMPLES

### Example 1
```
PS C:\> Stop-RestartManagerProcess -Force
```

Forcibly stops processes using any previously registered resources after a system-defined timeout if they don't successfully shut down.

## PARAMETERS

### -Force
Forcibly stop processes using any previously registered resources if they do not shut down before a system-defined timeout.

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

### -OnlyRegistered
Only shut down processes that have registered with the Restart Manager using RegisterApplicationRestart.

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
[Restart-RestartManagerProcess](Restart-RestartManagerProcess)
[RmShutdown](https://msdn.microsoft.com/library/windows/desktop/aa373667.aspx)
[RegisterApplicationRestart](https://msdn.microsoft.com/library/windows/desktop/aa373347.aspx)