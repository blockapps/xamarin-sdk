using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Contracts;
using BlockAppsSDK.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Contracts
{
    [TestClass]
    public class ContractTests
    {
        public ContractManager ContractManager { get; set; }
        public UserManager UserManager { get; set; }

        [TestMethod]
        public async Task CanCreateContract()
        {
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            var SimpleStorage = await ContractManager.CreateContract<SimpleStorageState>(src, "SimpleStorage", "test", "test",
                "094fec31a8a92735af93cb910b6de2cb9ed49d60");

            Assert.IsNotNull(SimpleStorage);
        }

        [TestMethod]
        public async Task CanGetAllContractAddressesWithName()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            var noContractAddresses = await ContractManager.GetContractAddresses("SimpleStorageWrong");
            Assert.IsTrue(contractAddresses.Length > 0);
            Assert.IsTrue(noContractAddresses.Length == 0);

        }

        [TestMethod]
        public async Task CanGetAllContractsWithName()
        {
            var contracts = await ContractManager.GetContractsWithName<SimpleStorageState>("SimpleStorage");
            var nullContracts = await ContractManager.GetContractsWithName<SimpleStorageState>("NotADeployedContract");
            Assert.IsNotNull(contracts);
            Assert.IsTrue(contracts.Count > 0);
            Assert.IsTrue(nullContracts.Count == 0);
        }

        [TestMethod]
        public async Task CanGetAContractWithAddress()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract<SimpleStorageState>("SimpleStorage", 
                contractAddresses[0]);

            Assert.IsNotNull(SimpleStorage);
        }

        [TestMethod]
        public async Task CanRefreshContract()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract<SimpleStorageState>("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(await SimpleStorage.RefreshContract());
        }

        [TestMethod]
        public async Task CanCallMethodOnContract()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract<SimpleStorageState>("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(SimpleStorage);
            string resp = null;
            var args = new Dictionary<string, string>();
            if (SimpleStorage.State.StoredData == 1)
            {
                args.Add("x", "0");
                resp = await SimpleStorage.CallMethod("set", args, "test", "test",
                    "094fec31a8a92735af93cb910b6de2cb9ed49d60", 3);
                Console.WriteLine(resp);
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.State.StoredData, 0);
            }
            else
            {
                args.Add("x", "1");
                resp = await SimpleStorage.CallMethod("set", args, "test", "test",
                 "094fec31a8a92735af93cb910b6de2cb9ed49d60", 3);
                Console.WriteLine(resp);
                Assert.AreEqual(SimpleStorage.State.StoredData, 0);
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.State.StoredData, 1);
            }
            
            
        }


        [TestMethod]
        public async Task CanCreateContractGeneric()
        {
            var user = await UserManager.CreateUser("charlie", "test");
            if (user == null)
            {
                user = await UserManager.GetUser("charlie", "test");
            }
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            var simpleStorage = await ContractManager.CreateContract<SimpleStorageState>(src, "SimpleStorage", user.Name, "test",
                user.DefaultAccount);

            Assert.IsNotNull(simpleStorage);
        }

        [TestInitialize]
        public void SetupManagers()
        {
            UserManager = new UserManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com/eth/v1.2"));
            ContractManager = new ContractManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com/eth/v1.2")); 
        }
    }
}
