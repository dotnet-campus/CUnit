# How to Contribute
## Contributing to Code
There are many ways to contribute to the Code project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

After cloning and building the repo, check out the [issues list](https://github.com/dotnet-campus/MSTestEnhancer/issues). Issues are good candidates to pick up if you are in the code for the first time.

## Build and Run
If you want to understand how Code works or want to debug an issue, you'll want to get the source, build it, and run the tool locally.

### Getting the sources
    git clone https://github.com/dotnet-campus/MSTestEnhancer.git

### Prerequisites
+ [Git](https://git-scm.com/)
+ [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
+ [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/)
+ [Unit Test](https://msdn.microsoft.com/en-us/library/dd264975.aspx)
+ [MStest V2](https://github.com/Microsoft/testfx)

Finally, install MSTest V2 packages using Nuget:
    
    Install-Package MSTest.TestFramework

### Build
+ Building with Visual Studio(VS)

You can open /MSTest.Extensions.sln in VS and trigger a build of the entire code base using Build Solution(Ctrl+Shift+B) from the Solution Explorer or the Build menu.

The bits get dropped at /bin/Debug/.

+ Building with command line(CLI)

        MSBuild.exe /MSTest.Extensions.sln

### Test
+ Running tests with Visual Studio

All the tests in the MSTestEnhancer repo can be run via the Visual Studio Test Explorer. Building /MSTest.Extensions.sln as described in the build section above should populate all the tests in the Test Explorer.

+ Running tests with command line(CLI)

        vstest.console.exe /tests/MSTest.Extensions.Tests/bin/Debug/net47/MSTest.Extensions.Tests.dll


