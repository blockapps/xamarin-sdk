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
        public bool Send(string toAddress)
        {
            throw new NotImplementedException();
        }
    }

    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}