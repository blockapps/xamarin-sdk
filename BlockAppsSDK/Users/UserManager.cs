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
        public Connection Connection { get; }
        public UserManager(Connection connection)
        {
            Connection = connection;
        } 

        public async Task<User> CreateUser(string name, string password)
        {
            var users = await GetAllUserNames();
            if (users.Contains(name))
            {
                return null;
            }
            var newUser = new User(Connection, name);
            var accountAddress = await newUser.AddNewAccount(password);

            newUser.SetSigningAccount(accountAddress, password);

            return newUser;
        }

        public async Task<User> GetUser(string name)
        {
            var user = new User(Connection, name);

            await user.PopulateAccounts();

            return user;
        }

        public async Task<List<string>> GetAllUserNames()
        {
            var res = await Utils.GET(Connection.BlocUrl + "/users");
            return JsonConvert.DeserializeObject<List<string>>(res);
        }

    }
}
