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

        public string SigningAccount { get; private set; }

        public Dictionary<string,Account> Accounts { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public BoundContractManager BoundContractManager { get; private set; }

        //Constructors
        public User(Connection connection, string name)
        {
            Name = name;
            AccountManager = new AccountManager(connection);
            BoundContractManager = new BoundContractManager(connection)
            {
                Username = name 
            };
        }

        //Methods
        public void SetSigningAccount(string address, string password)
        {
            SigningAccount = address;
            Accounts[address].Password = password;
            BoundContractManager.SigningAddress = address;
            BoundContractManager.SigningPassword = password;
        }

        public async Task<string> AddNewAccount(string password)
        {
            if (Accounts == null)
            {
                Accounts = new Dictionary<string, Account>();
            }
            var newAccount = await AccountManager.CreateAccount(Name, password, true);
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
            var transaction = await Accounts[SigningAccount].Send(toAddress, value, Name);
            return transaction;
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