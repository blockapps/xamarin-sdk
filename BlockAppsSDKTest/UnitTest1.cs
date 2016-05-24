using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockAppsSDK.Users;

namespace BlockAppsSDKTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var address = "3d726b4b0f75e242d4f0c8ebbef438d41dd1fe33";
            var account = await Account.GetAccount(address);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            BlockAppsSDK.BlockAppsSDK.BlocUrl = "http://13.93.154.77:8000";
            var account = await Account.GetAccountAddresses();
            var accounts = await Account.GetAccounts("admin");
            var x = "sup";
        }


    }
}
