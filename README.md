# BlockAppsSDK - Strato/Bloc API Client PCL for Xamarin


BlockAppsSDK is a client, portable class library targeting Xamarin for Visual Studio 4.0 and above. It provides a simple
way to interact with the Strato and Bloc API's with native C# classes.

## Prerequisites
  * [Git](https://git-scm.com/)
  * Running [bloc](https://github.com/blockapps/bloc) server
  * (Optional) Personal [Strato](https://azure.microsoft.com/en-us/marketplace/partners/consensys/blockapps-strato/) instance*

  \*We offer a development strato instance at: http://strato-dev3.blockapps.net/eth/v1.1/
## Usage examples

Get Block 0.
```c#
//Assuming local bloc server
ConnectionString.BlocUrl = "http://localhost:8000";
//BlockApps strato dev instance
ConnectionString.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";

var block = await Block.GetBlock(0);

Console.WriteLine("Block 0 has Parent Hash: " + block.BlockData.ParentHash);
```


Create a new User and a print the balance of the newly created account.

```c#
//Assuming local bloc server
ConnectionString.BlocUrl = "http://localhost:8000";
///BlockApps strato dev instance
ConnectionString.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";

var testUser = await User.CreateUser("testUser", "securePassword");

Console.WriteLine(testUser.Name + " has account:" + testUser.Accounts[0].Address + " with balance: " + testUser.Accounts[0].Balance);
```

Create a simple smart contract using our new user, testUser, and call
one of the contract's functions.

```c#
//Assuming local bloc server
ConnectionString.BlocUrl = "http://localhost:8000";
//BlockApps strato dev instance
ConnectionString.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";

var userTask = User.GetUser("testUser", "securePassword");

var contractString =
    "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";

var user = await userTask;
var contractTask = Contract.DeployContract(contractString, "SimpleStorage", user, user.Accounts[0]);

var contract = await contractTask;
Console.WriteLine("The value of storedData is: " + contract.Properties["storedData"]);

var args = new Dictionary<string, string>();
args.Add("x", "10");

var resp = await contract.CallMethod("set", args, user, user.Accounts[0].Address, 1);

//Refresh the contract object to have the changes made in our method call
await contract.Refresh();

Console.WriteLine("The new value of storedData is: " + contract.Properties["storedData"]);
```
## Supported Platforms

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
####Workaround 1:

After adding the package, if you do not see `BlockAppsSDK` under the `References -> From Packages` in the Solution Explorer
then the .dll must be manually added to the project.

Navigate to  `Project menu -> Edit References`

![edit_ref](https://github.com/blockapps/xamarin-sdk/blob/master/images/edit_ref.png?raw=true)

Select the `.Net Assembly` tab and click `Browse...`

![dotnet_asm](https://github.com/blockapps/xamarin-sdk/blob/master/images/dotnet_assembly.png?raw=true)

In the project directory select `packages` folder, then select the folder with the version of `BlockAppsSDK`
installed in the project. Click the `bin` folder.

![find_bin](https://github.com/blockapps/xamarin-sdk/blob/master/images/bin_location.png?raw=true)

Once in `bin` open the `Alpha` folder and select `BlockAppsSDK.dll` and click `Open`.

![find_dll](https://github.com/blockapps/xamarin-sdk/blob/master/images/dll_location.png?raw=true)

This will add the .dll file to the project references.
You may need to refresh the Solution Explorer to see the changes.

####Workaround 2: 

If you try to run you're project and get this error:
`Could not load file or assembly 'System.Net.Http, Version=1.5.0.0` the following steps
will resolve the problem:

Expand `References` and right click on `System.Net.Http`. If `Local Copy` is not
checked, select it to check it.

![system.net](https://github.com/blockapps/xamarin-sdk/blob/master/images/systemnethttp.png?raw=true)

####Workaround 3:

After adding the package, if you do not see `BlockAppsSDK` under the `References` in the Solution Explorer
then, the .dll must be manually added to the project.

Right click `References` and select `Add Reference...`

![add_ref](https://github.com/blockapps/xamarin-sdk/blob/master/images/add_ref_win.png?raw=true)

In Reference Manager select `Browse`. A list of .dll files will be displayed. Check the box for 
`BlockAppsSDK.dll`. Click `Okay`. You should now see BlockAppsSDK listed as a reference.

![add_ref_win](https://github.com/blockapps/xamarin-sdk/blob/master/images/add_dll_win.png?raw=true)

Once added you will now be able to reference the BlockAppsSDK namespaces in you project.

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
