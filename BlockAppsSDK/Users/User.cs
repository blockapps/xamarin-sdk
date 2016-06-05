using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Users
{
    public class User
    {
        //Properties
        public string Name { get; set; }

        public string Password { get; set; }

        public List<Account> Accounts { get; set; }

        //Static Methods
        public static async Task<User> CreateUser(string name, string password)
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
            newUser.Accounts.Add(await Account.CreateAccount(name, password, true));
            return newUser;
        }

        public static async Task<User> GetUser(string name, string password)
        {
            return new User
            {
                Name = name,
                Password = password,
                Accounts = await Account.GetAccounts(name)
            };
        }

        public static async Task<List<string>> GetAllUserNames()
        {
            var res = await Utils.GET(ConnectionString.BlocUrl + "/users");
            return JsonConvert.DeserializeObject<List<string>>(res);
        }
    }

    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}