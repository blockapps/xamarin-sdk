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
        [TestMethod]
        public async Task CreateAndGetAccount()
        {
            //ConnectionString.StratoUrl = "http://xamarintest.centralus.cloudapp.azure.com/strato-single/eth/v1.1/";
            //ConnectionString.BlocUrl = "http://xamarintest.centralus.cloudapp.azure.com:8000";
            ConnectionString.BlocUrl = "http://13.93.154.77:8000";
            //Create an Account to query
            var newAccount = await Account.CreateAccount("accountTest", "test", true);
            var account = await Account.GetAccount(newAccount.Address);
            Assert.AreEqual(newAccount, account);
        }

        [TestMethod]
        public async Task GetAllUserAccounts()
        {
            //ConnectionString.StratoUrl = "http://xamarintest.centralus.cloudapp.azure.com/strato-single/eth/v1.1/";
            //ConnectionString.BlocUrl = "http://xamarintest.centralus.cloudapp.azure.com:8000";
            ConnectionString.BlocUrl = "http://13.93.154.77:8000";
            //Create an Account to query
            var allAccountAddresses = await Account.GetAccountAddresses();
            List<Task<Account>> allAccountsTask = allAccountAddresses.Select(async addr => await Account.GetAccount(addr)).ToList();
            
            var allAccounts = (await Task.WhenAll(allAccountsTask)).ToList();
            Assert.IsNotNull(allAccounts);
        }
    }
}
