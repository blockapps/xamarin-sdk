using System;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Blocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Blocks
{
    [TestClass]
    public class BlockTests
    {
        [TestMethod]
        public async Task GetBlock()
        {
            ConnectionString.StratoUrl = "http://xamarintest.centralus.cloudapp.azure.com/strato-single/eth/v1.1/";
            ConnectionString.BlocUrl = "http://xamarintest.centralus.cloudapp.azure.com:8000";
            var block0 = await Block.GetBlock(0);
            Console.WriteLine("Block 0 has Parent Hash: " + block0.BlockData.ParentHash);
        }
    }
}
