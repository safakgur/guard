**Running the Build Scripts**

You need PowerShell or PowerShell Core.

* Windows: You already have `powershell`.
* Linux: See [Installing PowerShell Core on Linux][1] on Microsoft Docs for `pwsh`.
* macOS: Run `brew cask install powershell` for `pwsh`.

**Install the .NET Core CLI** - `install-dotnet.ps1`

* Downloads portable .NET Core SDKs 2.1.400 and 1.1.9 to the "artifacts/dotnet".
* Skips the download if these versions already exist in the folder.

**Build the Library** - `build.ps1`

* Automatically calls `install-dotnet.ps1`.
* Builds the project into the "artifacts/build".

Options

`-NoInstall`: If set, the build script won't call `install-dotnet.ps1`.

`-Configuration`: "Debug" or "Release". The default is "Release".

`-Framework`: "netstandard2.0" or "netstandard1.0". The default is "netstandard2.0".

**Run the Unit Tests** - `test.ps1`

* Automatically calls `install-dotnet.ps1`.
* Builds the project in place and runs the unit tests.
* Optionally, collects test coverage using the awesome [coverlet][2].

Options

`-NoInstall`: If set, the build script won't call `install-dotnet.ps1`.

`-Configuration`: "Debug" or "Release". The default is "Release".

`-Logger`: Specifies a logger for test results.

`-Coverage`: If set, produces test coverage.

`-CoverageFormat`: Output format of the coverage results. The default is "opencover".

`-CoverageOutput`: The output path of the coverage results. The default is "artifacts/coverage.xml".

**Use Your Existing .NET Core CLI**

The build and test scripts assume that `dotnet` is in PATH when `-NoInstall` is set and the SDKs
are not found in the "artifacts/dotnet".

[1]: https://docs.microsoft.com/powershell/scripting/setup/installing-powershell-core-on-linux
[2]: https://github.com/tonerdo/coverlet
