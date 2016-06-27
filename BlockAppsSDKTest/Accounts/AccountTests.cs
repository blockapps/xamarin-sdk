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
        public async Task CanCreateThenGetSameAccount()
        {
            //Create an Account to query
            var newAccount = await AccountManager.CreateAccount("accountTest", "test", true);
            var account = await AccountManager.GetAccount(newAccount.Address);
            Assert.AreEqual(newAccount.Address, account.Address);
            Assert.AreEqual(newAccount.Balance, account.Balance);
            Assert.AreEqual(newAccount.ContractRoot, account.ContractRoot);
        }

        [TestMethod]
        public async Task CanGetAllAccountsForUsers()
        {
            var allAccountAddresses = await AccountManager.GetAccountAddressesForAllUsers();
            Assert.IsNotNull(allAccountAddresses);
            //List<Task<Account>> allAccountsTask = allAccountAddresses.Select(async addr => await AccountManager.GetAccount(addr)).ToList();
            foreach (var user in allAccountAddresses)
            {
                foreach (var address in user.Value)
                {
                    var account = AccountManager.GetAccount(address);
                    Assert.IsNotNull(account);
                }
            }

            //var allAccounts = (await Task.WhenAll(allAccountsTask)).ToList();
            //Assert.IsNotNull(allAccounts);
        }

        [TestMethod]
        public async Task CanSend()
        {
            
            var charlieAccount = await AccountManager.GetAccount("219e43441e184f16fb0386afd3aed1e780632042");
            var accountTest = await AccountManager.GetAccount("3468276182f204ebe569fa572e8cee697f3379fd");
            Assert.IsNotNull(charlieAccount);
            Assert.IsNotNull(accountTest);
            var balanceCharlie = charlieAccount.Balance;
            var balanceTest = accountTest.Balance;
            var transaction =  await accountTest.Send(charlieAccount.Address, 10, "accountTest", "test");
            Assert.AreEqual(transaction.Value, "10000000000000000000");
            await Task.WhenAll(new Task[2] {charlieAccount.Refresh(), accountTest.Refresh()});
            Assert.IsTrue(double.Parse(balanceCharlie) < double.Parse(charlieAccount.Balance));
            Assert.IsTrue(double.Parse(balanceTest) > double.Parse(accountTest.Balance));
        }

        [TestInitialize]
        public void SetupManagers()
        {
           AccountManager = new AccountManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2")); 
        }
    }
}
