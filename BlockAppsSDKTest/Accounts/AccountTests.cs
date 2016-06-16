using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Accounts
{
    [TestClass]
    public class AccountTests
    {
        private AccountManager AccountManager { get; set; }
        [TestMethod]
        public async Task CreateAndGetAccount()
        {
            //Create an Account to query
            var newAccount = await AccountManager.CreateAccount("accountTest", "test", true);
            var account = await AccountManager.GetAccount(newAccount.Address);
            Assert.AreEqual(newAccount, account);
        }

        [TestMethod]
        public async Task GetAllUserAccounts()
        {
            //Create an Account to query
            var allAccountAddresses = await AccountManager.GetAccountAddresses();
            List<Task<Account>> allAccountsTask = allAccountAddresses.Select(async addr => await AccountManager.GetAccount(addr)).ToList();
            
            var allAccounts = (await Task.WhenAll(allAccountsTask)).ToList();
            Assert.IsNotNull(allAccounts);
        }

        [TestInitialize]
        public void SetupManagers()
        {
           AccountManager = new AccountManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2")); 
        }
    }
}
