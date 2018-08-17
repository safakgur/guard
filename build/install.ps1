$SdkPath = "$PSScriptRoot/../artifacts/dotnet";
$GetSdk = "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; " +
    "&([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1'))) " +
    "-Channel Current -InstallDir $SdkPath -NoPath"

$Shell = "powershell"
if (Get-Command "pwsh" -ErrorAction SilentlyContinue) 
{ 
   $Shell = "pwsh"
}

& $Shell -NoProfile -ExecutionPolicy unrestricted -Command $GetSdk
