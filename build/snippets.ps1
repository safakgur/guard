param([switch]$NoInstall=$false)

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

& $SdkPath restore $PSScriptRoot/../utils --verbosity m
& $SdkPath run -p $PSScriptRoot/../utils/ -c Release snippets $PSScriptRoot/../snippets

exit $lastexitcode
