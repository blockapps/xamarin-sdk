using System;
using BlockAppsSDK;
using BlockAppsSDK.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Contracts
{
    [TestClass]
    public class ContractTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestInitialize]
        public void SetupManagers()
        {
            = new AccountManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2")); 
        }
    }
}
