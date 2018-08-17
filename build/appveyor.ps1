IF ($Env:APPVEYOR_BUILD_WORKER_IMAGE -eq "Ubuntu")
{
	& $PSScriptRoot/test.ps1 -NoInstall -Configuration Release
}
ELSE
{
    & $PSScriptRoot/test.ps1 -NoInstall -Configuration CI -Coverage
	choco install codecov --no-progress
	codecov -f artifacts/coverage.xml
}
