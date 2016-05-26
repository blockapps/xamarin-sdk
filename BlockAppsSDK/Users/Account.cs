using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Users
{
    public class Account
    {
        //Properties
        public string ContractRoot { get; set; }

        public string Kind { get; set; }

        public string Balance { get; set; }

        public string Address { get; set; }

        public string LatestBlockNum { get; set; }

        public string LatestBlockId { get; set; }

        //Methods
        public bool Send(string address, uint value, string user)
        {
            throw new NotImplementedException();
        }


        //Static Methods
        public static async Task<Account> GetAccount(string address)
        {
            if (address == null || address.Equals(""))
            {
                throw new ArgumentException("Address is null or empty", nameof(address));
            }
            var url = ConnectionString.StratoUrl + "/account?address=" + address;
            var accountList = JsonConvert.DeserializeObject<List<Account>>(await Utils.GET(url));
            if (accountList.Count > 0)
            {
                return accountList.First();
            }
            else
            {
                return null;
            }
        }

        public static async Task<List<Account>> GetAccounts(string username)
        {
            var addresses = JsonConvert.DeserializeObject<string[]>(await Utils.GET(ConnectionString.BlocUrl + "/users/" + username));
            List<Task<string>> accountTasks = (from address in addresses
                                               select Utils.GET(ConnectionString.StratoUrl + "account?address=" + address)).ToList();
            List<string> accountJsonList = (await Task.WhenAll(accountTasks)).ToList();

            return accountJsonList.Select(x => JsonConvert.DeserializeObject<Account[]>(x)[0]).ToList();
        }

        public static async Task<List<string>> GetAccountAddresses()
        {
            var res = await Utils.GET(ConnectionString.BlocUrl + "/users");
            var users = JsonConvert.DeserializeObject<List<string>>(res);
            List<Task<string>> userTasks = (from user in users
                                               select Utils.GET(ConnectionString.BlocUrl + "/users/" + user)).ToList();
            return (await Task.WhenAll(userTasks)).ToList();
        }

        public static async Task<Account> CreateAccount(string name, string password, bool faucet)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("Name is null or empty", nameof(name));
            }
            var url = ConnectionString.BlocUrl + "/users/" + name;
            var postModel = new PostNewUserModel
            {
                password = password,
                faucet = faucet ? "1" : "0"
            };
            var serializedModel = JsonConvert.SerializeObject(postModel);
            var userAddress = await Utils.POST(url, serializedModel);
            return await GetAccount(userAddress);
        }
    }

}
