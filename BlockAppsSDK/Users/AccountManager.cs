using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Users
{
    public class AccountManager
    {
         public Connection Connection { get; private set; }

        public AccountManager(Connection connection)
        {
            Connection = connection;
        }

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
                return accountList.First();
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Account>> GetAccounts(string username)
        {
            var addresses = JsonConvert.DeserializeObject<string[]>(await Utils.GET(Connection.BlocUrl + "/users/" + username));
            List<Task<string>> accountTasks = (from address in addresses
                                               select Utils.GET(Connection.StratoUrl + "/account?address=" + address)).ToList();
            List<string> accountJsonList = (await Task.WhenAll(accountTasks)).ToList();

            if (accountJsonList.Count < 1)
            {
                return null;
            }

            return accountJsonList.Select(x => JsonConvert.DeserializeObject<Account[]>(x)[0]).ToList();
        }

        public async Task<List<string>> GetAccountAddresses()
        {
            var res = await Utils.GET(Connection.BlocUrl + "/users");
            var users = JsonConvert.DeserializeObject<List<string>>(res);
            //List<Task<string>> userTasks = (from user in users
            //                                   select Utils.GET(Connection.BlocUrl + "/users/" + user)).ToList();
            List<Task<string>> userTasks = users.Select(async user => JsonConvert.DeserializeObject<string>(await Utils.GET(Connection.BlocUrl + "/users/" + user))).ToList();
            return (await Task.WhenAll(userTasks)).ToList();
        }

        public async Task<Account> CreateAccount(string name, string password, bool faucet)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("Name is null or empty", nameof(name));
            }
            var url = Connection.BlocUrl + "/users/" + name;
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
