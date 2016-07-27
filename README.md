# BlockAppsSDK - Strato/Bloc API Client PCL for Xamarin


BlockAppsSDK is a client, portable class library targeting Xamarin for Visual Studio 4.0 and above. It provides a simple
way to interact with the Strato and Bloc API's with native C# classes.

## Prerequisites
  * [Git](https://git-scm.com/)
  * Running [bloc](https://github.com/blockapps/bloc) server
  * (Optional) Personal [Strato](https://azure.microsoft.com/en-us/marketplace/partners/consensys/blockapps-strato/) instance*

  \*We offer a development strato instance at: http://strato-dev4.blockapps.net/eth/v1.2/
## Usage examples

Get Block 2.
```c#
 //Assuming there is a local bloc server running that is pointing to the BlockApps
 //strato-dev4 server
 var BAClient = new BlockAppsClient("http://localhost:8000",
     "http://strato-dev4.blockapps.net/eth/v1.2");
 var block2 = await BAClient.BlockManager.GetBlock(2);
 Console.WriteLine("Block 2 has nonce: " + block2.BlockData.Nonce);
```


Create a new User and a print the balance of the newly created account associated with that User.

```c#
var BAClient = new BlockAppsClient("http://localhost:8000",
    "http://strato-dev4.blockapps.net/eth/v1.2");

//Create a new user 'test' with secret key 'test'
//Then check the balance of the default account set to user
var newUser = await BAClient.UserManager.CreateUser("test", "test");
if (newUser == null)
{
    Console.WriteLine("User test already exists in Bloc");
}
else
{
    Console.WriteLine("New User " + newUser.Name + "created");
    Console.WriteLine(newUser.Name + " has " + newUser.Accounts[newUser.DefaultAccount].Balance + " wei in their default account.");
}
```

Create a simple smart contract using our new user, `test`, and call
one of the contract's functions.

```c#
var BAClient = new BlockAppsClient("http://localhost:8000",
    "http://strato-dev4.blockapps.net/eth/v1.2");

var newUser = await BAClient.UserManager.CreateUser("test", "test");
if (newUser == null)
{
    newUser = await BAClient.UserManager.GetUser("test", "test");
}
var src =
    "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";

//Create the contract bound to a user.
var contract = await newUser.BoundContractManager.CreateBoundContract(src, "SimpleStorage");
Console.WriteLine("The value of storedData is " + contract.Properties["storedData"]);

//Call the `set` method on the contract to change the value of storedData.
var args = new Dictionary<string, string>();
args.Add("x", "3");

//Here we specify the name of the method, the arguments as a dictionary and the amount of ether we would like to send
//to the contract.
var returnMsg = await contract.CallMethod("set", args, 1);

//Refresh the contract since we have updated its state
await contract.RefreshAccount();

 Console.WriteLine("The value of storedData has changed to " + contract.Properties["storedData"]);
}
```
## Supported Platforms

* .NET 4.5
* Xamarin.iOS / Xamarin.Android / Xamarin.Mac
* Mono 3.x

## Getting Started

The package is available as a pre-release package on NuGet. From the Package
manager console enter:

```
Install-Package BlockAppsSDK -Pre
```

In **Xamarin Studio (Mac)** you can find the package by navigating to `Project menu -> Add
NuGet Packages...`

![project_addpkg](https://github.com/blockapps/xamarin-sdk/blob/master/images/project_addpkg.png?raw=true)

Check the *Show pre-release packages* box and search `BlockAppsSDK`. This should
result in the following screen.

![search_package](https://github.com/blockapps/xamarin-sdk/blob/master/images/add_pkg.png?raw=true)

Click `Add Package`. Xamarin Studio will install the package dependencies into the project.

If you have any issues building after adding the package please see **Workaround 1** and 
**Workaround 2** in the **Problems** section.

In **Visual Studio (Windows)** you can find the package by navigating to `Project menu -> Manage
NuGet Packages...`

![mng_pkg](https://github.com/blockapps/xamarin-sdk/blob/master/images/mng_pkg_win.png?raw=true)

Select the *Browse* tab and check the *Include prerelease* box. Search for `BlockAppsSDK`.

![install_pkg](https://github.com/blockapps/xamarin-sdk/blob/master/images/install_pkg_win.png?raw=true)

Click `Install` and the BlockAppsSDK will be added to your project. Please see **Workaround 3** under problems
to resolve a common installation issue with our package.

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

  * Press the windows key + s and type Developer Command Prompt.
  * Navigate to the directory where you cloned the project.
```
cd xamarin-sdk/BlockAppsSDK
MSBuild BlockAppsSDK.csproj
```

## Contribute

Contribution Guidelines are TBD at the moment but coming soon.

## Problems?

Please see our [Workarounds Page](https://github.com/blockapps/xamarin-sdk/blob/master/XamarinInstallIssues.md) for common issues with installing or building with the BlockAppsSDK.


###Bugs
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
