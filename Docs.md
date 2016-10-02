# BlockAppsSDK - Documentation

### BlockAppsClient
The `BlockAppsClient` Class is used to set the URLs for both the `bloc-server`
and `strato-api`. Both are RESTful API's that the `BlockAppsClient` abstracts
to native C# classes.
#### Usage
```c#
// Replace `bloc-url` and `strato-url with their respective URLs`
var blockAppsClient = new BlockAppsClient("http://bloc-url", "http://strato-url");
```
The `BlockAppsClient` has 3 properties; `UserManager`, `BlockManager`,
`ContractManager`. The following sections will describes how to use these in
detail.

### `BlockManager`
The `BlockManager` queries blocks on the blockchain. Currently it only supports
requesting blocks by number.

#### Usage
```c#
var BlockManager = blockAppsClient.BlockManager;
```

##### `GetBlock(int number)`
```c#
var block1 = await BlockManager.GetBlock(1);
```

### `UserManager`
The `UserManager` - creates and gets users registered in the bloc-server.

#### Usage
```c#
var UserManager = blockAppsClient.UserManager;
```

##### `CreateUser(string name, string password)` 
Creates a user with a given username. An account is created with `password` argument. This account is set as the
`SigningAccount` property on the user instance.
```c#
var user = await UserManager.CreateUser("username");
```

##### `GetUser(string name)` 
Gets a user according to their username
```c#
var user = await UserManager.GetUser("username");
```

#####  `GetAllUserNames()`
Gets a list of registered the usernames
```c#
var user = await UserManager.GetAllUserNames();
```

### `User`
`User`'s own `Account`'s that are represent Ether accounts. They are
used to  send Ether, create Contract, or call Contract methods.

#### Usage
```c#
var UserManager = blockAppsClient.UserManager;
var user = await UserManager.CreateUser("username","password");
```

##### `SetSigningAccount(string address, string password)`
Set the account that is bound to newly created contracts and that is used with the `Send` method.
`address` is the address of the account and `password` is the accounts password.
```c#
user.SetSigningAccount("username","password");
```

##### `AddNewAccount(string password)` 
Adds a new account to the user's `Accounts` porperty.
```c#
var newAddress = await user.AddNewAccount("secretPassword");
```

##### `PopulateAccounts()` 
Populates the `Accounts` property with all the Accounts associated with the user.
```c#
await user.PopulateAccounts();
```

##### `Send(string toAddress, uint value)`
Sends the given `value` of Ether to the `toAddress`. The Ether is sent from the `SigningAccount`.
```c#
var transaction = await user.Send("deadbeef", value);
```

##### `RefreshAllAccounts()` 
Refreshes all the accounts in the `Accounts` property.
```c#
await user.RefreshAllAccounts();
```

### `user.BoundContractManager`
The `BoundContractManager` creates and gets `BoundContracts`. `BoundContract`'s
are `Contract`'s that are bound to an account that will sign all transactions
required for interacting with the contract on the chain.

#### Usage
```c#
var UserManager = blockAppsClient.UserManager;
var user = await UserManager.CreateUser("username","password");
```

#####  `CreateBoundContract<T>(string src, string contractName)` 
Takes contract source and the name of the contract, and deploys it to the blockchain. Returns
an instance of the `BoundContract`. The generic class is a model of the state
variables within the solidity contract.

```c#
public class SimpleStorageState
{
    public int StoredData { get; set; }
}
```
```c#
var source = @"
contract SimpleStorage {
  uint storedData;

  function set(uint x) {
    storedData = x;
  }

  function get() returns (uint retVal) {
    return storedData;
  }
}"
var contract = await user.BoundContractManager.CreateBoundContract<SimpleStorageState>(source, "SimpleStorage")
```

##### `GetBoundContract<T>(string contractName, string address)` 
Creates an instance of a boundContract with a given name and address.
```c#
var contract = await user.BoundContractManager.GetBoundContract<SimpleStorageState>("SimpleStorage","deadbeef")
```

##### `GetBoundContractsWithName<T>(string contractName)` 
Gets all the Contracts of a given name and instantiates them as `BoundContract`'s
```c#
var listOfContracts = await user.BoundContractManager.GetBoundContractsWithName<SimpleStorageState>("SimpleStorage")
```

### `BoundContract`
The `BoundContract` class inherits the `Contract` class. `BoundContract`'s
handle signing, holding the username, address, and password associated with
the account the contract is bound.

#### Usage
```c#
var UserManager = blockAppsClient.UserManager;
var user = await UserManager.CreateUser("username","password");
var boundContract = await user.BoundContractManager.GetBoundContract<SimpleStorageState>("SimpleStorage","deadbeef");
```

##### `SetSigningAccount(string address, string password)` 
Set the signing account for the bound contract.
```c#
boundContract.SetSigningAccount("deadbeef", "beefy-password");
```

##### `CallMethod(string methodName, Dictionary<string, string> args, double value)` 
Calls the method of the contract with the given name and arguments. The `value`
parameter is the amount of ether that is sent to the contract in transaction to
call method.

```c#
var args = new Dictionary<string, string>();
args.Add("x", "2");
var resp = await boundContract.CallMethod("set", args, 3)
```
