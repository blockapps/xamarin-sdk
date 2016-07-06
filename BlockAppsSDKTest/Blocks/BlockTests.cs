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
            BlockManager = new BlockManager(new Connection("http://40.118.255.235:8000",
                 "http://40.118.255.235/eth/v1.2"));
        }
    }
}
