using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Users
{
    public class UserManager
    {
        private AccountManager AccountManager { get; set; }

        public UserManager(AccountManager accountManager)
        {
            AccountManager = accountManager;
        } 

        public async Task<User> CreateUser(string name, string password)
        {
            var users = await GetAllUserNames();
            if (users.Contains(name))
            {
                return null;
            }
            var newUser = new User
            {
                Name = name,
                Password = password,
                Accounts = new List<Account>()
            };
            newUser.Accounts.Add(await AccountManager.CreateAccount(name, password, true));
            return newUser;
        }

        public async Task<User> GetUser(string name, string password)
        {
            return new User
            {
                Name = name,
                Password = password,
                Accounts = await AccountManager.GetAccounts(name)
            };
        }

        

        public async Task<List<string>> GetAllUserNames()
        {
            var res = await Utils.GET(AccountManager.Connection.BlocUrl + "/users");
            return JsonConvert.DeserializeObject<List<string>>(res);
        }

    }
}
