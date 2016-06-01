using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAppsSDK.Users
{
    public class User
    {
        //Properties
        public string Name { get; set; }

        public string Password { get; set; }

        public List<Account> Accounts { get; set; }

        //Methods
        public async Task<User> CreateUser(string name, string password)
        {
            var newUser = new User
            {
                Name = name,
                Password = password,
                Accounts = new List<Account>()
            };
            newUser.Accounts.Add(await Account.CreateAccount(name, password, true));
            return newUser;
        }


        //Static Methods
        public static async Task<User> GetUser(string name, string password)
        {
            return new User
            {
                Name = name,
                Password = password,
                Accounts = await Account.GetAccounts(name)
            };
        }
    }

    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}