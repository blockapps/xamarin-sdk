using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Blocks;
using Newtonsoft.Json;
using BlockAppsSDK.Contracts;

namespace BlockAppsSDK.Users
{
    public class User
    {
        //Properties
        public string Name { get; set; }

        public string Password {private get; set; }

        public string DefaultAccount { get; private set; }

        public Dictionary<string,Account> Accounts { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public BoundContractManager BoundContractManager { get; private set; }

        //Constructors
        public User(Connection connection, string name, string password)
        {
            Name = name;
            Password = password;
            AccountManager = new AccountManager(connection);
            BoundContractManager = new BoundContractManager(connection)
            {
                Password = password,
                Username = name 
            };
        }

        //Methods
        public void SetDefaultAccount(string address)
        {
            DefaultAccount = address;
            BoundContractManager.DefaultAddress = address;
        }

        public async Task<string> AddNewAccount()
        {
            if (Accounts == null)
            {
                Accounts = new Dictionary<string, Account>();
            }
            var newAccount = await AccountManager.CreateAccount(Name, Password, true);
            Accounts.Add(newAccount.Address, newAccount);
            return newAccount.Address;
        }

        public async Task PopulateAccounts()
        {
            var accounts = await AccountManager.GetAccountsForUser(Name);
            Accounts = accounts.ToDictionary(x => x.Address);
        }

        public async Task<AccountTransaction> Send(string toAddress, uint value)
        {
            var transaction = await Accounts[DefaultAccount].Send(toAddress, value, Name, Password);
            return transaction;
        }

        public async Task<AccountTransaction> Send(string toAddress, string fromAddress, uint value)
        {
            if (Accounts.ContainsKey(fromAddress))
            {
                var transaction = await Accounts[fromAddress].Send(toAddress, value, Name, Password);
                return transaction;
            }
            return null;
        }

        public async Task RefreshAllAccounts()
        {
            await Task.WhenAll(Accounts.Select(x => x.Value.RefreshAccount()).ToList());
        }
    }


    public class PostNewUserModel
    {
        //Properties
        public string password { get; set; }

        public string faucet { get; set; }
    }
}