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
       
    }

    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}