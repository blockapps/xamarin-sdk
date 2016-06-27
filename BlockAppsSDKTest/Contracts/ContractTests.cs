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

        [TestMethod]
        public async Task CanCreateContract()
        {
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            var SimpleStorage = await ContractManager.CreateContract(src, "SimpleStorage", "charlie", "test",
                "219e43441e184f16fb0386afd3aed1e780632042");

            Assert.IsNotNull(SimpleStorage);
            Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
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
            var contracts = await ContractManager.GetContractsWithName("SimpleStorage");
            var nullContracts = await ContractManager.GetContractsWithName("NotADeployedContract");
            Assert.IsNotNull(contracts);
            Assert.IsTrue(contracts.Count > 0);
            Assert.IsNull(nullContracts);
        }

        [TestMethod]
        public async Task CanGetAContractWithAddress()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract("SimpleStorage", 
                contractAddresses[0]);

            Assert.IsNotNull(SimpleStorage);
            Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
        }

        [TestMethod]
        public async Task CanRefreshContract()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(await SimpleStorage.Refresh());
        }

        [TestMethod]
        public async Task CanCallMethodOnContract()
        {
            var contractAddresses = await ContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await ContractManager.GetContract("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(SimpleStorage);
            string resp = null;
            var args = new Dictionary<string, string>();
            if (SimpleStorage.Properties["storedData"].Equals("1"))
            {
                args.Add("x", "0");
                resp = await SimpleStorage.CallMethod("set", args, "charlie", "test",
                    "219e43441e184f16fb0386afd3aed1e780632042", 3);
                Console.WriteLine(resp);
                await SimpleStorage.Refresh();
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
            }
            else
            {
                args.Add("x", "1");
                resp = await SimpleStorage.CallMethod("set", args, "charlie", "test",
                 "219e43441e184f16fb0386afd3aed1e780632042", 3);
                Console.WriteLine(resp);
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
                await SimpleStorage.Refresh();
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "1");
            }
            
            
        }


        [TestInitialize]
        public void SetupManagers()
        {
           ContractManager = new ContractManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2")); 
        }
    }
}
