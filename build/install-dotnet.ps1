$Dir = "$PSScriptRoot/../artifacts/dotnet";

function Install-Sdk {
    param([string]$Version)
    IF ($IsWindows -or $env:OS -like "Windows*")
    {
        $Get = "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; " +
            "&([scriptblock]::Create((Invoke-WebRequest -useb 'https://dot.net/v1/dotnet-install.ps1')))"

        $Shell = "powershell"
        if (Get-Command "pwsh" -ErrorAction SilentlyContinue) 
        { 
           $Shell = "pwsh"
        }

        & $Shell -NoProfile -ExecutionPolicy unrestricted -Command $Get -Channel Current -Version $Version -InstallDir $Dir -NoPath
    }
    ELSE
    {
        curl -s https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel Current --version $Version --install-dir $Dir --no-path
    }
}

Install-Sdk -Version "2.1.400"
Install-Sdk -Version "1.1.9"
