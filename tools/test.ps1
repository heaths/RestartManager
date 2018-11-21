# Copyright (c) 2017 Heath Stewart
# See the LICENSE.txt file in the project root for more information.

[CmdletBinding()]
param (
    [Parameter()]
    [ValidateNotNullOrEmpty()]
    [string] $Configuration = $env:CONFIGURATION,

    [Parameter()]
    [ValidateNotNullOrEmpty()]
    [string] $Platform = $env:PLATFORM
)

if (-not $Configuration) {
    $Configuration = 'Debug'
}

if (-not $Platform) {
    $Platform = 'AnyCPU'
}

[string] $solutionDir = Resolve-Path "$PSScriptRoot\.."
[string] $outDir = "$solutionDir\bin\$Configuration"
[string[]] $assemblies = Get-ChildItem -Path "$solutionDir\test\*\bin\$Configuration" -Filter '*.Test.dll' -Recurse

if (!(Test-Path "$outDir")) {
    $null = New-Item "$outDir" -ItemType Directory
}

$targetargs = "$assemblies -noshadow -xml `"$outDir\RestartManager.results.xml`""
if ($VerbosePreference -eq 'Continue') {
    $targetargs += ' -verbose'
}

[string[]] $filter = @(
    '+[RestartManager*]*'
    '-[RestartManager*]*.Properties.Resources'
    '-[RestartManager*.Test]*'
)

& "$solutionDir\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -register:user -target:"$solutionDir\packages\xunit.runner.console.2.3.1\tools\net452\xunit.console.exe" -targetargs:"$targetargs" -returntargetcode -filter:"$filter" -excludebyattribute:"*.ExcludeFromCodeCoverage*" -output:"$outDir\RestartManager.coverage.xml"
if ($LASTEXITCODE) {
    $FailedCount = $LASTEXITCODE
}

& "$solutionDir\packages\ReportGenerator.4.0.4\tools\net47\ReportGenerator.exe" "-reports:$outDir\RestartManager.coverage.xml" "-targetdir:$outDir" -reporttypes:Cobertura
if ($FailedCount) {
    throw "Failed $FailedCount tests"
}
