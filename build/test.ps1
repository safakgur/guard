param(
    [switch]$NoInstall=$false,
    [ValidateSet("Debug","Release","CI")][string]$Configuration="Release",
    [switch]$Coverage=$false,
    [ValidateSet("json","lcov","opencover","cobertura")][string]$CoverageFormat="opencover",
    [string]$CoverageOutput="../artifacts/coverage.xml"
)

IF ($NoInstall -eq $false)
{
    & $PSScriptRoot/install-dotnet.ps1
}

$SdkPath = "$PSScriptRoot/../artifacts/dotnet/dotnet"
IF ($IsWindows -or $env:OS -like "Windows*")
{
    $SdkPath += ".exe"
}

IF (-not (Test-Path $SdkPath))
{
    $SdkPath = "dotnet"
}

& $SdkPath restore $PSScriptRoot/../tests --verbosity m
& $SdkPath test $PSScriptRoot/../tests/ `
    -c $Configuration `
    /p:DebugType=full `
    /p:CollectCoverage=$Coverage `
    /p:CoverletOutputFormat=$CoverageFormat `
    /p:CoverletOutput="$CoverageOutput"

exit $lastexitcode
