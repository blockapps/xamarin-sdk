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
                return null;
            }

            var url = Connection.StratoUrl + "/account?address=" + address;

            var getResult = await Utils.GET(url);

            var accountList = JsonConvert.DeserializeObject<List<Account>>(getResult);

            if (accountList.Count > 0)
            {
                var account =  accountList.First();
                account.Connection = Connection;

                //bug: This is a fix for a bug in strato where leading 0 is missing from address
                if (account.Address.Length < 40)
                {
                    var zeroes = new String('0',(40- account.Address.Length));
                    account.Address = zeroes + account.Address;
                }
                return account;
            }
                return null;
        }

        public async Task<List<Account>> GetAccountsForUser(string username)
        {
            var res = await Utils.GET(Connection.BlocUrl + "/users/" + username);

            if (res.Equals("InternalServerError"))
            {
                return null;
            }

            var addresses = JsonConvert.DeserializeObject<string[]>(res);
            for (var i = 0; i < addresses.Length; i++)
            {
                if (addresses[i].Length < 40)
                {
                    var zeroes = new String('0', (40 - addresses[i].Length));
                    addresses[i] = zeroes + addresses[i];
                }
            }
            List<Task<Account>> accountTasks = (from address in addresses
                                               select GetAccount(address)).ToList();
            List<Account> accounts = (await Task.WhenAll(accountTasks)).ToList();
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
            for (var i = 0; i < userAddresses.Count; i++)
            {
                for (var j = 0; j < userAddresses[i].Value.Count; j++)
                {
                    if (userAddresses[i].Value[j].Length < 40)
                    {
                        var zeroes = new String('0', (40 - userAddresses[i].Value[j].Length));
                        userAddresses[i].Value[j] = zeroes + userAddresses[i].Value[j];
                    }
                }
            }

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

            var postData = new JObject(new JProperty("password", password), new JProperty("faucet", faucetValue)).ToString();
            var userAddress = await Utils.POST(url, postData);

            if (userAddress.Length < 40)
            {
                var zeroes = new String('0', (40 - userAddress.Length));
                userAddress = zeroes + userAddress;
            }

            var newAccount = await GetAccount(userAddress);
            newAccount.Password = password;
            return newAccount;
        }
    }
}
