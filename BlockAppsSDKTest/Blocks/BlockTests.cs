using System;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Blocks;
using BlockAppsSDK.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Blocks
{
    [TestClass]
    public class BlockTests
    {
        private BlockManager BlockManager { get; set; }

        [TestMethod]
        public async Task GetBlock()
        {
            var block0 = await BlockManager.GetBlock(1);
            Console.WriteLine("Block 0 has Parent Hash: " + block0.BlockData.ParentHash);
        }

        [TestInitialize]
        public void SetupManagers()
        {
            BlockManager = new BlockManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                 "http://tester9.centralus.cloudapp.azure.com/eth/v1.2"));
        }
    }
}
