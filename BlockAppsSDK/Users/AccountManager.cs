using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockAppsSDK.Users
{
    public class AccountManager
    {
        public Connection Connection { get; }

        public AccountManager(Connection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Get a specific Account using it's address. No association with a specific user.
        /// </summary>
        /// <param name="address">
        /// The address associated with an account on the blockchain.
        /// </param>
        public async Task<Account> GetAccount(string address)
        {
            if (address == null || address.Equals(""))
            {
                throw new ArgumentException("Address is null or empty", nameof(address));
            }
            var url = Connection.StratoUrl + "/account?address=" + address;
            var accountList = JsonConvert.DeserializeObject<List<Account>>(await Utils.GET(url));
            if (accountList.Count > 0)
            {
                var account =  accountList.First();
                account.Connection = Connection;
                return account;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Account>> GetAccountsForUser(string username)
        {
            var addresses = JsonConvert.DeserializeObject<string[]>(await Utils.GET(Connection.BlocUrl + "/users/" + username));
            List<Task<string>> accountTasks = (from address in addresses
                                               select Utils.GET(Connection.StratoUrl + "/account?address=" + address)).ToList();
            List<string> accountJsonList = (await Task.WhenAll(accountTasks)).ToList();

            if (accountJsonList.Count < 1)
            {
                return null;
            }

            var accounts = accountJsonList.Select(x => JsonConvert.DeserializeObject<Account[]>(x)[0]).ToList();
            foreach (var account in accounts)
            {
                account.Connection = Connection;
            }
            return accounts;
        }

        public async Task<Dictionary<string, List<string>>> GetAccountAddressesForAllUsers()
        {
            var res = await Utils.GET(Connection.BlocUrl + "/users");
            var users = JsonConvert.DeserializeObject<List<string>>(res);
            if (users.Count < 1)
            {
                return null;
            }
            //List<Task<string>> userTasks = (from user in users
            //                                   select Utils.GET(Connection.BlocUrl + "/users/" + user)).ToList();
            List<Task<KeyValuePair<string,List<string>>>> userTasks = users.Select(async user => new KeyValuePair<string,List<string>>(user,JsonConvert.DeserializeObject<List<string>>(await Utils.GET(Connection.BlocUrl + "/users/" + user)))).ToList();
            var userAddresses =  (await Task.WhenAll(userTasks)).ToList();
            return userAddresses.ToDictionary(x => x.Key, x => x.Value);


        }

        public async Task<Account> CreateAccount(string name, string password, bool faucet)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("Name is null or empty", nameof(name));
            }
            var url = Connection.BlocUrl + "/users/" + name;
            //var postModel = new PostNewUserModel
            //{
            //    password = password,
            //    faucet = faucet ? "1" : "0"
            //};
            var faucetValue = faucet ? "1" : "0";
            var postData = "{}";

            postData = new JObject(new JProperty("password", password), new JProperty("faucet", faucetValue)).ToString();
            var userAddress = await Utils.POST(url, postData);
            return await GetAccount(userAddress);
        }
    }
}
