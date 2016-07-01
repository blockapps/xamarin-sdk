using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlockAppsSDK.Blocks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockAppsSDK.Users
{
    public class Account
    {
        //Properties
        public string ContractRoot { get; set; }

        public string Kind { get; set; }

        public string Balance { get; set; }

        public string Address { get; set; }

        public string LatestBlockNum { get; set; }

        public string LatestBlockId { get; set; }

        public Connection Connection { get; set; }

        //Constructor
        public Account() { }

        public Account(Connection connection)
        {
            Connection = connection;
        }

        //Methods
        public async Task<AccountTransaction> Send(string toAddress, uint value, string user, string password)
        {
            var url = Connection.BlocUrl + "/users/" + user + "/" + Address + "/send";

            var postData = new JObject(new JProperty("password", password), new JProperty("toAddress", toAddress), new JProperty("value", value)).ToString();

            var resp = await Utils.POST(url, postData);

            var transaction = JsonConvert.DeserializeObject<AccountTransaction>(resp);
            await RefreshAccount();

            return transaction;

        }

        public async Task RefreshAccount()
        {
            var url = Connection.StratoUrl + "/account?address=" + Address;
            var accountList = JsonConvert.DeserializeObject<List<Account>>(await Utils.GET(url));
            if (accountList.Count > 0)
            {
                var refreshedAccount = accountList.First();
                ContractRoot = refreshedAccount.ContractRoot;
                Kind = refreshedAccount.Kind;
                Balance = refreshedAccount.Balance;
                LatestBlockId = refreshedAccount.LatestBlockId;
                LatestBlockNum = refreshedAccount.LatestBlockNum;
            }
        }
       
    }

    public class AccountTransaction
    {
        public string Hash { get; set; }
        public string GasLimit { get; set; }
        public string Data { get; set; }
        public string GasPrice { get; set; }
        public string CodeOrData { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public string From { get; set; }
        public string r { get; set; }
        public string s { get; set; }
        public string v { get; set; }
        public string Nonce { get; set; }
    }

}
