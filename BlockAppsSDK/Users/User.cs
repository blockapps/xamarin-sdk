using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BlockAppsSDK.Contracts;

namespace BlockAppsSDK.Users
{
    public class User
    {
        //Properties
        public string Name { get; set; }

        public string Password {private get; set; }

        public string DefaultAccount { get; set; }

        public Dictionary<string,Account> Accounts { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public ContractManager UserContractManager { get; private set; }

        //Constructors
        public User(Connection connection, string name, string password)
        {
            Name = name;
            Password = password;
            AccountManager = new AccountManager(connection);
            UserContractManager = new UserContractManager(connection)
            {
                Password = password,
                Username = name 
            };
        }

        public async Task<string> AddNewAccount()
        {
            var newAccount = await AccountManager.CreateAccount(Name, Password, true);
            Accounts.Add(newAccount.Address, newAccount);
            return newAccount.Address;
        }

        public async Task PopulateAccounts()
        {
            var accounts = await AccountManager.GetAccounts(Name);
            Accounts = accounts.ToDictionary(x => x.Address);
            return;
        }

    }


    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}