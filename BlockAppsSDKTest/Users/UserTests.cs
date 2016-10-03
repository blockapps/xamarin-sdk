using System;
using System.Linq;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Users
{
    [TestClass]
    public class UserTests
    {
        public UserManager UserManager { get; set; }
        public string NewUser { get; set; }
        public string Password { get; set; } = "test";

        [TestMethod]
        public async Task CanCreateUser()
        {
            var userObject = await UserManager.CreateUser(NewUser, Password);
            Assert.IsNotNull(userObject);
            Assert.IsNotNull(userObject.Accounts);
        }

        [TestMethod]
        public async Task CanGetUser()
        {
            var test = await UserManager.GetUser("test");
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Accounts);
        }

        [TestMethod]
        public async Task CanGetAllUserNames()
        {
            var usernames = await UserManager.GetAllUserNames();
            Assert.IsNotNull(usernames);
        }

        [TestMethod]
        public async Task CanAddNewAccount()
        {

            var test = await UserManager.GetUser("test");
            var numOfAccounts = test.Accounts.Count;
            await test.AddNewAccount("test");
            Assert.IsTrue(numOfAccounts < test.Accounts.Count);
        }

        [TestMethod]
        public async Task CanPopulateAccounts()
        {
            var test = await UserManager.GetUser("test");
            var numOfAccounts = test.Accounts.Count;
            var accountManager = new AccountManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                     "http://tester9.centralus.cloudapp.azure.com"));
            await accountManager.CreateAccount("test", "test", true);
            await test.PopulateAccounts();
            Assert.IsTrue(numOfAccounts < test.Accounts.Count);
        }

        [TestMethod]
        public async Task CanSend()
        {
            var test = await UserManager.GetUser("test");
            test.SetSigningAccount(test.Accounts.FirstOrDefault().Value.Address, "test");
            var accountTest = await UserManager.GetUser("accountTest");
            accountTest.SetSigningAccount(accountTest.Accounts.FirstOrDefault().Value.Address, "test");

            Assert.IsNotNull(test);
            Assert.IsNotNull(accountTest);
            var balancetest = test.Accounts[test.SigningAccount].Balance;
            var balanceTest = accountTest.Accounts[accountTest.SigningAccount].Balance;
            var transaction = await accountTest.Send(test.SigningAccount, 10);
            await test.Accounts[test.SigningAccount].RefreshAccount();
            Assert.AreEqual(transaction.Value, "10000000000000000000");
            Assert.IsTrue(double.Parse(balancetest) < double.Parse(test.Accounts[test.SigningAccount].Balance));
            Assert.IsTrue(double.Parse(balanceTest) > double.Parse(accountTest.Accounts[accountTest.SigningAccount].Balance));
        }

        [TestMethod]
        public async Task CanRefreshAllAccounts()
        {
            var test = await UserManager.GetUser("test");
            await test.RefreshAllAccounts();
        }

        [TestInitialize]
        public void SetupManagers()
        {
            if (UserManager == null)
            {
                UserManager = new UserManager(new Connection("http://tester9.centralus.cloudapp.azure.com:8000",
                     "http://tester9.centralus.cloudapp.azure.com"));
            }
            if (NewUser == null)
            {
                NewUser = Guid.NewGuid().ToString();
            }
        }
    }
}
