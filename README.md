# BlockAppsSDK - Strato/Bloc API Client PCL for Xamarin


BlockAppsSDK is a client, portable class library targeting Xamarin for Visual Studio 4.0 and above. It provides a simple
way to interact with the Strato and Bloc API's with native C# classes.

## Usage examples

Get Block 0.
```c#
//Assuming local bloc server
ConnectionStrings.BlocUrl = "http://localhost:8000";
//BlockApps strato dev instance
ConnectionStrings.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";
var block = await Block.GetBlock(0);
Console.WriteLine("Block 0 has Parent Hash: " + block.ParentHash);
```


Create a new User and a print the balance of the newly created account.

```c#
//Assuming local bloc server
ConnectionStrings.BlocUrl = "http://localhost:8000";
//BlockApps strato dev instance
ConnectionStrings.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";
var user = await User.CreateUser("newUserName", "secretPassword");
Console.WriteLine("New User has account:" + user.Accounts[0].Address + " with balance: " + user.Accounts[0].Balance);
```

## Supported Platforms

* Xamarin.iOS / Xamarin.Android / Xamarin.Mac
* Mono 3.x

## Getting Started

The package is available as a pre-release package on NuGet

```
Install-Package BlockAppsSDK -Pre
```


In Xamarin Studio you can find this option under the Project menu -> Add
Nuget Packages...

## Documentation

Documentation is in the works!

## Build

BlockAppsSDK is an assembly built to deploy anywhere. Should you
wish to compile it yourself, here's what you will need:

* Visual Studio 2015 or Xamarin Studio

To clone it locally click the "Clone in Desktop" button above or run the
following git commands.

```
git clone https://github.com/blockapps/xamarin-sdk.git

```

You will then need to open and build the project from Visual Studio
or use the Visual Studio developer console:

```
cd xamarin-sdk
devenv BlockAppsSDK.sln /build
```

## Contribute

Contribution Guidelines are TBD at the moment but coming soon.

## Problems?

BlockAppsSDK for Xamarin is in alpha stage and may contain bugs. If you do find
any please visit the
[issue tacker](https://github.com/blockapps/xamarin-sdk/issues) and
report the problem.

Please be kind and search to see if the issue is already logged before creating
a new one. If you're pressed for time, log it anyways.

When creating an issue, clearly explain

* What you were trying to do.
* What you expected to happen.
* What actually happened.
* Steps to reproduce the problem.

Also include any other information you think is relevant to reproduce the
problem.

## License



Licensed under the [Apache 2
License](http://www.apache.org/licenses/LICENSE-2.0)
