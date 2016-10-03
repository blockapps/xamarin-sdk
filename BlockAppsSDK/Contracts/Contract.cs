using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlockAppsSDK.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockAppsSDK.Contracts
{
    public class Contract<T> : Account
    {
        //Properties
        public string Name { get; set; }
        public T State { get; set; }

        //Constructor
        public Contract(Account account) : base(account.Connection)
        {
            ContractRoot = account.ContractRoot;
            Kind = account.Kind;
            Balance = account.Balance;
            Address = account.Address;
            LatestBlockNum = account.LatestBlockNum;
            LatestBlockId = account.LatestBlockId;
        }

        //Methods
        public async Task<string> CallMethod(string methodName, Dictionary<string, string> args, string username,
            string password, string userAddress, double value)
        {
            var url = Connection.BlocUrl + "/users/" + username + "/" + userAddress + "/contract/" + this.Name + "/" +
                      this.Address + "/call";
            var postData = "{}";
            if (args != null)
            {
                postData = new JObject(new JProperty("password", password), new JProperty("method", methodName),
                    new JProperty("args", JObject.FromObject(args)), new JProperty("value", value)).ToString();
            }

            return await Utils.POST(url, postData);

        }

        public async Task<bool> RefreshContract()
        {
            var accountRefresh = RefreshAccount();
            var stateTask = GetContractState(Address);
            var state = await stateTask;
            if (state == null)
            {
                await accountRefresh;
                return false;
            }
            State = state;
            await accountRefresh;
            return true;
        }

        

        private async Task<T> GetContractState(string address)
        {
            var url = Connection.BlocUrl + "/contracts/" + Name + "/" + address + "/state";
            try
            {
                var responseContent = await Utils.GET(url);
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                var contractState = JsonConvert.DeserializeObject<T>(responseContent, jsonSerializerSettings);


                return contractState;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }




        }

        public class DeployContractModel
        {
            public string password { get; set; }

            public string src { get; set; }
        }


    }
}
