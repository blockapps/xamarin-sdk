using System;
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
            var charlie = await UserManager.GetUser("charlie", "test");
            Assert.IsNotNull(charlie);
            Assert.IsNotNull(charlie.Accounts);
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

            var charlie = await UserManager.GetUser("charlie", "test");
            var numOfAccounts = charlie.Accounts.Count;
            await charlie.AddNewAccount();
            Assert.IsTrue(numOfAccounts < charlie.Accounts.Count);
        }

        [TestMethod]
        public async Task CanPopulateAccounts()
        {
            var charlie = await UserManager.GetUser("charlie", "test");
            var numOfAccounts = charlie.Accounts.Count;
            var accountManager = new AccountManager(new Connection("http://40.118.255.235:8000",
                     "http://40.118.255.235/eth/v1.2"));
            await accountManager.CreateAccount("charlie", "test", true);
            await charlie.PopulateAccounts();
            Assert.IsTrue(numOfAccounts < charlie.Accounts.Count);
        }

        [TestMethod]
        public async Task CanSend()
        {
            var charlie = await UserManager.GetUser("charlie", "test");
            var accountTest = await UserManager.GetUser("accountTest", "test");

            Assert.IsNotNull(charlie);
            Assert.IsNotNull(accountTest);
            var balanceCharlie = charlie.Accounts[charlie.DefaultAccount].Balance;
            var balanceTest = accountTest.Accounts[accountTest.DefaultAccount].Balance;
            var transaction = await accountTest.Send(charlie.DefaultAccount, 10);
            await charlie.Accounts[charlie.DefaultAccount].Refresh();
            Assert.AreEqual(transaction.Value, "10000000000000000000");
            Assert.IsTrue(double.Parse(balanceCharlie) < double.Parse(charlie.Accounts[charlie.DefaultAccount].Balance));
            Assert.IsTrue(double.Parse(balanceTest) > double.Parse(accountTest.Accounts[accountTest.DefaultAccount].Balance));
        }

        [TestMethod]
        public async Task CanRefreshAllAccounts()
        {
            var charlie = await UserManager.GetUser("charlie", "test");
            await charlie.RefreshAllAccounts();
        }

        [TestInitialize]
        public void SetupManagers()
        {
            if (UserManager == null)
            {
                UserManager = new UserManager(new Connection("http://40.118.255.235:8000",
                     "http://40.118.255.235/eth/v1.2"));
            }
            if (NewUser == null)
            {
                NewUser = Guid.NewGuid().ToString();
            }
        }
    }
}
