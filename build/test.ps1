param(
    [switch]$NoInstall=$false,
    [string]$Configuration="Release",
    [switch]$Coverage=$false,
    [string]$CoverageFormat="opencover",
    [string]$CoverageOutput="../artifacts/coverage.xml"
)

IF ($NoInstall -eq $false)
{
    & $PSScriptRoot/install-dotnet.ps1
}

$SdkPath = "../artifacts/dotnet/dotnet.exe"
IF (-not (Test-Path $SdkPath))
{
    $SdkPath = "dotnet"
}

& $SdkPath restore $PSScriptRoot/../src --verbosity m
& $SdkPath test $PSScriptRoot/../tests/ `
    /p:DebugType=full `
    /p:CollectCoverage=$Coverage `
    /p:CoverletOutputFormat=$CoverageFormat `
    /p:CoverletOutput="$CoverageOutput"
