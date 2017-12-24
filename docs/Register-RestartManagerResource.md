---
external help file: RestartManager.PowerShell.dll-Help.xml
Module Name: RestartManager
online version: https://github.com/heaths/RestartManager/wiki/Register-RestartManagerResource
schema: 2.0.0
---

# Register-RestartManagerResource

## SYNOPSIS
Registers one or more file, process, or service with the Restart Manager.

## SYNTAX

### Path (Default)
```
Register-RestartManagerResource [[-Path] <String[]>] [-ServiceName <String[]>] [-Process <Process[]>]
 [-Session <RestartManagerSession>]
```

### LiteralPath
```
Register-RestartManagerResource [-LiteralPath <String[]>] [-ServiceName <String[]>] [-Process <Process[]>]
 [-Session <RestartManagerSession>]
```

## DESCRIPTION
Registers one or more file, process, or service with the active Restart Manager session in $RestartManagerSession or passed to -Session. You should minimize the number of times you call this cmdlet by passing as many resources - even a mix of file, process, or service objects - as possible to a single invocation. Piping objects to this cmdlet will batch them for a single call at the end of the pipeline.

## EXAMPLES

### Example 1
```
PS C:\> dir .\MyApp -filter *.dll -recurse | Register-RestartManagerResource
```

Registers all DLLs under the .\MyApp folder with the active Restart Manager session in $RestartManagerSession.

### Example 2
```
PS C:\> Get-Process devenv | Register-RestartManagerResource
```

Registers any and all processes named 'devenv' with the active Restart Manager session in $RestartManagerSession.

### Example 3
```
PS C:\> Get-Service MyService | Register-RestartManagerResource
```

Registers the service 'MyService' with the active Restart Manager session in $RestartManagerSession.

## PARAMETERS

### -LiteralPath
The path to a file or files to register with the Restart Manager. Wildcards are not supported.

```yaml
Type: String[]
Parameter Sets: LiteralPath
Aliases: PSPath

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Path
The path to a file or files to register with the Restart Manager. Wildcards are supported.

```yaml
Type: String[]
Parameter Sets: Path
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Process
One or more process to register with the Restart Manager.

```yaml
Type: Process[]
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ServiceName
One or more service name to register with the Restart Manager.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

## INPUTS

### System.String[]
### System.Diagnostics.Process[]


## RELATED LINKS
[Get-RestartManagerProcess](Get-RestartManagerProcess)
[RmRegisterResource](https://msdn.microsoft.com/library/windows/desktop/aa373663.aspx)