$SdkPath = "../artifacts/dotnet";
$GetSdk = "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; " +
    "&([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1'))) " +
    "-Channel Current -InstallDir $SdkPath -NoPath"

powershell -NoProfile -ExecutionPolicy unrestricted -Command $GetSdk
