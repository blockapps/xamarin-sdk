# BlockAppsSDK for Xamarin - Issue Workarounds

## Xamarin Studio (Mac)

#### Workaround 1:

After adding the package (please see [Getting Started](https://github.com/blockapps/xamarin-sdk) section to add the package), if you do not see `BlockAppsSDK` under the `References -> From Packages` in the Solution Explorer
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

#### Workaround 2:

If you try to run you're project and get this error:
`Could not load file or assembly 'System.Net.Http, Version=1.5.0.0` the following steps
will resolve the problem:

Expand `References` and right click on `System.Net.Http`. If `Local Copy` is not
checked, select it to check it.

![system.net](https://github.com/blockapps/xamarin-sdk/blob/master/images/systemnethttp.png?raw=true)


# Visual Studio 2015 (Windows)

#### Workaround 3:

After adding the package (please see [Getting Started](https://github.com/blockapps/xamarin-sdk) section to add the package), if you do not see `BlockAppsSDK` under the `References` in the Solution Explorer
then, the .dll must be manually added to the project.

Right click `References` and select `Add Reference...`

![add_ref](https://github.com/blockapps/xamarin-sdk/blob/master/images/add_ref_win.png?raw=true)

In Reference Manager select `Browse`. A list of .dll files will be displayed. Check the box for
`BlockAppsSDK.dll`. Click `Okay`. You should now see BlockAppsSDK listed as a reference.

![add_ref_win](https://github.com/blockapps/xamarin-sdk/blob/master/images/add_dll_win.png?raw=true)

Once added you will now be able to reference the BlockAppsSDK namespaces in you project.
