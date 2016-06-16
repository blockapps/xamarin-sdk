using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlockAppsSDK.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlockAppsSDK.Contracts
{
    public class Contract : Account
    {
        //Properties
        public string Name { get; set; }

        public List<string> Methods { get; set; }
        
        public Dictionary<string,string> Properties { get; set; }

        //Constructor
        public Contract(Account account)
        {
            ContractRoot = account.ContractRoot;
            Kind = account.Kind;
            Balance = account.Balance;
            Address = account.Address;
            LatestBlockNum = account.LatestBlockNum;
            LatestBlockId = account.LatestBlockId;
        }

        //Methods
        public async Task<string> CallMethod(string methodName, Dictionary<string,string> args, User user, string userAddress, double value)
        {
            var url = BlockAppsClient.BlocUrl + "/users/" + user.Name + "/" + userAddress + "/contract/" + this.Name + "/" + this.Address + "/call";
            var postData = "{}";
            if (args != null)
            {
                postData = new JObject(new JProperty("password", user.Password), new JProperty("method", methodName),
                    new JProperty("args", JObject.FromObject(args)), new JProperty("value", value)).ToString();
            }

            return await Utils.POST(url, postData);

        }

        public async Task<bool> Refresh()
        {
            var stateTask = GetContractState(Address, Name);
            var state = await stateTask;
            if (state == null)
            {
                return false;
            }

            Methods.Clear();
            Properties.Clear();
            foreach (var keyValuePair in state)
            {
                if (keyValuePair.Value.Contains("function"))
                {
                    Methods.Add(keyValuePair.Key);
                }
                else
                {
                    Properties.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return true;
        }


    }

    public class DeployContractModel
    {
        public string password { get; set; }

        public string src { get; set; }
    }

    

}
