using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockAppsSDK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest
{
    [TestClass]
    public class UsageExamples
    {
        [TestMethod]
        public async Task GetBLock()
        {
            var BAClient = new BlockAppsClient("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com");
            //var BAClient = new BlockAppsClient("http://localhost:8000",
            //    "http://strato-dev4.blockapps.net");
            var block2 = await BAClient.BlockManager.GetBlock(2);
            Console.WriteLine("Block 2 has nonce: " + block2.BlockData.Nonce);
        }

        [TestMethod]
        public async Task CreateUser()
        {
            var BAClient = new BlockAppsClient("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com");
            //var BAClient = new BlockAppsClient("http://localhost:8000",
            //    "http://strato-dev4.blockapps.net");
            
            //Create a new user 'test' with secret key 'test'
            //Then check the balance of the default account set to user test
            var newUser = await BAClient.UserManager.CreateUser("test", "test");
            if (newUser == null)
            {
                Console.WriteLine("User test already exists in Bloc");
            }
            else
            {
                Console.WriteLine("New User " + newUser.Name + "created");
                Console.WriteLine(newUser.Name + " has " + newUser.Accounts[newUser.SigningAccount].Balance + " wei in their default account.");
            }
        }

        [TestMethod]
        public async Task DeployContract()
        {
            var BAClient = new BlockAppsClient("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com");
            //var BAClient = new BlockAppsClient("http://localhost:8000",
            //    "http://strato-dev4.blockapps.net");

            var newUser = await BAClient.UserManager.CreateUser("test", "test");
            if (newUser == null)
            {
                newUser = await BAClient.UserManager.GetUser("test");
                newUser.SetSigningAccount(newUser.Accounts.FirstOrDefault().Value.Address,"test");
            }
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            
            //Create the contract bound to a user.
            var contract = await newUser.BoundContractManager.CreateBoundContract<SimpleStorageState>(src, "SimpleStorage");
            Console.WriteLine("The value of storedData is " + contract.State.StoredData);

            //Call the `set` method on the contract to change the value of storedData.
            var args = new Dictionary<string, string>();
            args.Add("x", "3");

            //Here we specify the name of the method, the arguments as a dictionary and the amount of ether we would like to send
            //to the contract.
            var returnMsg = await contract.CallMethod("set", args, 1);

            //Refresh the contract since we have updated its state
            await contract.RefreshAccount();

            Console.WriteLine("The value of storedData has changed to " + contract.State.StoredData);



        }


    }

    public class SimpleStorageState
    {
        public uint StoredData { get; set; }
    }
}
