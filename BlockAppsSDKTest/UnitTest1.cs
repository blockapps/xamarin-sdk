//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using BlockAppsSDK.Users;
//using BlockAppsSDK;
//using BlockAppsSDK.Contracts;


//namespace BlockAppsSDKTest
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        //[TestMethod]
//        //public async Task TestMethod1()
//        //{
//        //    var account = await Account.GetAccount(address);
//        //}

//        [TestMethod]
//        public async Task TestMethod2()
//        {
//            BlockAppsClient.BlocUrl = "http://13.93.154.77:8000";
//            var account = await Account.GetAccountAddressesForAllUsers();
//            var accounts = await Account.GetAccountsForUser("admin");
//            var x = "sup";
//        }

//        [TestMethod]
//        public async Task TestMethod3()
//        {
//            BlockAppsClient.BlocUrl = "http://13.93.154.77:8000";
//            var newAccount = await Account.CreateAccount("testMethod3", "asdf", true);
//            var x = "sup";
            
//        }

//        //[TestMethod]
//        //public async Task TestMethod4()
//        //{
//        //    BlockAppsClient.BlocUrl = "http://13.93.154.77:8000";
//        //    var contractAddress = await Contract.GetContractAddress("SimpleStorage");
//        //    var contractState = await Contract.GetContractState(address: "a4f5503c2e3f32bc567c1e654f6d69fec71f46ef", name: "SimpleStorage");
//        //    var contract = await Contract.GetContract("SimpleStorage");
           
//        //    var x = "sup";

//        //}

//        //[TestMethod]
//        //public async Task TestMethod5()
//        //{
//            //BlockAppsClient.BlocUrl = "http://13.93.154.77:8000";
//            //var userTask = User.GetUser("charlie", "test");
//            //var contractTask = Contract.GetContract("SimpleStorage");

//            //var user = await userTask;
//            //var contract = await contractTask;
//            //var args = new Dictionary<string,string>();
//            //args.Add("x","1");
//            ////args = null;
//            //var resp = await contract.CallMethod("set", args, user, user.Accounts[0].Address, 3);

//            //var x = "sup";

//        //}

//        [TestMethod]
//        public async Task TestMethod6()
//        {
//            BlockAppsClient.BlocUrl = "http://13.91.95.100:8000";
//            BlockAppsClient.StratoUrl = "http://13.91.95.100/eth/v1.2/";

//            var user = await User.CreateUser("charlie", "test");
//            if (user == null)
//            {
//                user = await User.GetUser("charlie", "test");
//            }
//            //var file =
//            //    "contract Task { /** * It is helpful to think of * smart contracts as state machine. * In this example: * State 1: Deploy new smart task contract * State 2: Set task name and reward * State 3: Task Completed * State 4: Task Deleted */ address owner; address completedBy; uint taskReward; string stateMessage; uint stateInt; string taskName; string taskDescription; function Task() { owner = msg.sender; stateMessage = \"Task uploaded\"; stateInt = 1; } /** * Set the details specific to this task */ function setUpTaskDetails(uint reward, string name, string description) returns (string){ if(reward >= ((this.balance + msg.value) / 1000000000000000000)) { msg.sender.send(msg.value); return \"Not enough ether sent as reward\"; } taskReward = reward; stateMessage = \"Task details set\"; taskName = name; taskDescription = description; stateInt = 2; return stateMessage; } /** * Complete the task contract */ function completeTask() returns (string){ completedBy = msg.sender; completedBy.send(taskReward * 1 ether); stateInt = 3; stateMessage = \"Task successfully completed\"; return stateMessage; } function deleteTask() returns (string){ owner.send(this.balance); stateInt = 4; stateMessage = \"Deleted Task\"; return stateMessage; } }";

//            //var contractTask = Contract.DeployContract(file, "Task", user, user.Accounts[0]);
//            ////var contractTask = Contract.GetContract("Task1");
//            ////var user = await userTask;
//            //var contract = await contractTask;
//            //var args = new Dictionary<string,string>();
//            //args.Add("reward","10");
//            //args.Add("name","Book surf trip");
//            //args.Add("description","John and I must go surf");
//            ////args = null;
//            //var resp = await contract.CallMethod("setUpTaskDetails", args, user, user.Accounts[0].Address, 1);

//            //var taskContracts = await Contract.GetContractsWithName("Task");

//            var x = "sup";

//        }

//        [TestMethod]
//        public async Task TestMethod7()
//        {
            
//            BlockAppsClient.BlocUrl = "http://13.93.154.77:8000";
//            var contractsTask = Contract.GetContractsWithName("Task11");
//            var resp = await contractsTask;

//            var x = "sup";
//        }

//        [TestMethod]
//        public async Task TestMethod8()
//        {
//            BlockAppsClient.StratoUrl = "http://xamarintest.centralus.cloudapp.azure.com/strato-single/eth/v1.1/";
//            BlockAppsClient.BlocUrl = "http://xamarintest.centralus.cloudapp.azure.com:8000";
//            var charlie = await User.GetUser("charlie", "test");
//            Assert.AreSame(charlie.Name,"charlie");
//            Assert.AreSame(charlie.Password,"test");
//            Assert.AreSame(charlie.Accounts.Count,1);
//        }

//        [TestMethod]
//        public async Task TestMethod9()
//        {
//            BlockAppsClient.BlocUrl = "http://xamarin.centralus.cloudapp.azure.com:8000";

//            //BlockApps strato dev instance
//            //BlockAppsClient.StratoUrl = "http://strato-dev3.blockapps.net/eth/v1.1/";
//            //var testUser = await User.CreateUser("testUser", "securePassword");
//            //if (testUser == null)
//            //{
//            //    Console.WriteLine("User already exists, add an account through the Account class.");
//            //}
//            //else
//            //{
//            //    Console.WriteLine(testUser.Name + " has account:" + testUser.Accounts[0].Address + " with balance: " + testUser.Accounts[0].Balance);
//            //}


//            //====================//
//            BlockAppsClient.StratoUrl = "http://xamarin.centralus.cloudapp.azure.com/eth/v1.2";
//            //var userTask = User.GetUser("testUser", "securePassword");
//            var userTask = User.GetUser("charlie", "test");
//            //var contractString =
//            //    "contract SimpleStorage4 { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
//            var contractString =
//                "contract Task { /** * It is helpful to think of * smart contracts as state machine. * In this example: * State 1: Deploy new smart task contract * State 2: Set task name and reward * State 3: Task Completed * State 4: Task Deleted */ address owner; address completedBy; uint taskReward; string stateMessage; uint stateInt; string taskName; string taskDescription; function Task() { owner = msg.sender; stateMessage = \"Task uploaded\"; stateInt = 1; } /** * Set the details specific to this task */ function setUpTaskDetails(uint reward, string name, string description) returns (string){ if(reward >= ((this.balance + msg.value) / 1000000000000000000)) { msg.sender.send(msg.value); return \"Not enough ether sent as reward\"; } taskReward = reward; stateMessage = \"Task details set\"; taskName = name; taskDescription = description; stateInt = 2; return stateMessage; } /** * Complete the task contract */ function completeTask() returns (string){ completedBy = msg.sender; completedBy.send(taskReward * 1 ether); stateInt = 3; stateMessage = \"Task successfully completed\"; return stateMessage; } function deleteTask() returns (string){ owner.send(this.balance); stateInt = 4; stateMessage = \"Deleted Task\"; return stateMessage; } }"; ;
//            //"contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";


//            var user = await userTask;
//            var contractTask = Contract.DeployContract(contractString, "Task", user, user.Accounts[0]);
//            var contract = await contractTask;
//            Console.WriteLine("The value of storedData is: " + contract.Properties["storedData"]);
//            var args = new Dictionary<string, string>();
//            args.Add("x", "10");
//            var resp = await contract.CallMethod("set", args, user, user.Accounts[0].Address, 1);
//            await contract.Refresh();
//            Console.WriteLine("The new value of storedData is: " + contract.Properties["storedData"]);

//        }

//    }
//}
