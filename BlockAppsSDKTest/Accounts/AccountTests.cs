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
        public async Task NoAccountsForUnregisteredUser()
        {
            var accounts = await AccountManager.GetAccountsForUser("not-a-user");
            Assert.IsNull(accounts);
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
            var newAccount = await AccountManager.CreateAccount("accountTest", "test", true);
            var newAccount2 = await AccountManager.CreateAccount("accountTest2", "test", true);
            
            Assert.IsNotNull(newAccount);
            Assert.IsNotNull(newAccount2);
            var balanceAccountTest = newAccount.Balance;
            var balanceAccountTest2 = newAccount2.Balance;
            var transaction = await newAccount2.Send(newAccount.Address, 10, "accountTest2");
            Assert.AreEqual(transaction.Value, "10000000000000000000");
            await Task.WhenAll(new Task[2] { newAccount.RefreshAccount(), newAccount2.RefreshAccount()});
            Assert.IsTrue(double.Parse(balanceAccountTest) < double.Parse(newAccount.Balance));
            Assert.IsTrue(double.Parse(balanceAccountTest2) > double.Parse(newAccount2.Balance));
        }

        [TestInitialize]
        public void SetupManagers()
        {
           AccountManager = new AccountManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                "http://tester9.centralus.cloudapp.azure.com")); 
        }
    }
}
