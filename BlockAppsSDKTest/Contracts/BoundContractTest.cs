using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Contracts
{
    [TestClass]
    public class BoundContractTest
    {
        public BoundContractManager BoundContractManager { get; set; }

        [TestMethod]
        public async Task CanCreateBoundContract()
        {
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            var SimpleStorage = await BoundContractManager.CreateBoundContract(src, "SimpleStorage",
                "219e43441e184f16fb0386afd3aed1e780632042");

            Assert.IsNotNull(SimpleStorage);
            Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
        }

        [TestMethod]
        public async Task CanGetAllBoundContractsWithName()
        {
            var boundContracts = await BoundContractManager.GetBoundContractsWithName("SimpleStorage");
            var nullContracts = await BoundContractManager.GetBoundContractsWithName("NotADeployedContract");
            Assert.IsNotNull(boundContracts);
            Assert.IsTrue(boundContracts.Count > 0);
            Assert.IsNull(nullContracts);
        }

        [TestMethod]
        public async Task CanGetABoundContractWithAddress()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var boundSimpleStorage = await BoundContractManager.GetBoundContract("SimpleStorage",
                contractAddresses[0]);

            Assert.IsNotNull(boundSimpleStorage);
            Assert.IsTrue(boundSimpleStorage.Properties["storedData"].Equals("0") || boundSimpleStorage.Properties["storedData"].Equals("1"));
        }

        [TestMethod]
        public async Task CanRefreshBoundContract()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await BoundContractManager.GetContract("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(await SimpleStorage.RefreshContract());
        }

        [TestMethod]
        public async Task CanCallBoundContractMethod()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await BoundContractManager.GetBoundContract("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(SimpleStorage);
            string resp = null;
            var args = new Dictionary<string, string>();
            if (SimpleStorage.Properties["storedData"].Equals("1"))
            {
                args.Add("x", "0");
                resp = await SimpleStorage.CallMethod("set", args, 3);
                Console.WriteLine(resp);
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
            }
            else
            {
                args.Add("x", "1");
                resp = await SimpleStorage.CallMethod("set", args, 3);
                Console.WriteLine(resp);
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.Properties["storedData"], "1");
            }

        }

        [TestInitialize]
        public void SetupManagers()
        {
            BoundContractManager = new BoundContractManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2"))
            {
                Username = "charlie",
                Password = "test",
                DefaultAddress = "219e43441e184f16fb0386afd3aed1e780632042"
            };

        }
    }
}
