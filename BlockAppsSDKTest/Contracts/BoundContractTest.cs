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
            var SimpleStorage = await BoundContractManager.CreateBoundContract<SimpleStorageState>(src, "SimpleStorage");

            Assert.IsNotNull(SimpleStorage);
            Assert.AreEqual(SimpleStorage.State.StoredData, 0);
        }

        [TestMethod]
        public async Task CanGetAllBoundContractsWithName()
        {
            var boundContracts = await BoundContractManager.GetBoundContractsWithName<SimpleStorageState>("SimpleStorage");
            var nullContracts = await BoundContractManager.GetBoundContractsWithName<object>("NotADeployedContract");
            Assert.IsNotNull(boundContracts);
            Assert.IsTrue(boundContracts.Count > 0);
            Assert.IsTrue(nullContracts.Count == 0);
        }

        [TestMethod]
        public async Task CanGetABoundContractWithAddress()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var boundSimpleStorage = await BoundContractManager.GetBoundContract<SimpleStorageState>("SimpleStorage",
                contractAddresses[0]);

            Assert.IsNotNull(boundSimpleStorage);
            Assert.IsTrue(boundSimpleStorage.State.StoredData.Equals(0) || boundSimpleStorage.State.StoredData.Equals(1));
        }

        [TestMethod]
        public async Task CanRefreshBoundContract()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await BoundContractManager.GetContract<SimpleStorageState>("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(await SimpleStorage.RefreshContract());
        }

        [TestMethod]
        public async Task CanCallBoundContractMethod()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var SimpleStorage = await BoundContractManager.GetBoundContract<SimpleStorageState>("SimpleStorage",
                contractAddresses[0]);
            Assert.IsNotNull(SimpleStorage);
            string resp = null;
            var args = new Dictionary<string, string>();
            if (SimpleStorage.State.StoredData.Equals(1))
            {
                args.Add("x", "0");
                resp = await SimpleStorage.CallMethod("set", args, 3);
                Console.WriteLine(resp);
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.State.StoredData, 0);
            }
            else
            {
                args.Add("x", "1");
                resp = await SimpleStorage.CallMethod("set", args, 3);
                Console.WriteLine(resp);
                Assert.AreEqual(SimpleStorage.State.StoredData, 0);
                await SimpleStorage.RefreshContract();
                Assert.AreEqual(SimpleStorage.State.StoredData, 1);
            }

        }

        [TestInitialize]
        public void SetupManagers()
        {
            BoundContractManager = new BoundContractManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com"))
            {
                Username = "test",
                SigningPassword = "test",
                SigningAddress = "094fec31a8a92735af93cb910b6de2cb9ed49d60"
            };

        }
    }

    public class SimpleStorageState
    {
        public int StoredData { get; set; }
    }

}
