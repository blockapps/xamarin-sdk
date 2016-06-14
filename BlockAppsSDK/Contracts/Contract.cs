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
            var url = ConnectionString.BlocUrl + "/users/" + user.Name + "/" + userAddress + "/contract/" + this.Name + "/" + this.Address + "/call";
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

        public static async Task<Contract> DeployContract(string src, string contractName, User user, Account account)
        {
            var url = ConnectionString.BlocUrl + "/users/" + user.Name + "/" + account.Address + "/contract";

            var postModel = new DeployContractModel
            {
                password = user.Password,
                src = src
            };
            var serializedModel = JsonConvert.SerializeObject(postModel);
            var responseContent = await Utils.POST(url, serializedModel);
            return await GetContract(contractName, responseContent);

            
        }

        public static async Task<string[]> GetContractAddresses(string name)
        {
            var hexPatter = @"^[0-9A-Fa-f]+$";
            var url = ConnectionString.BlocUrl + "/contracts/" + name;
            var responseContent = await Utils.GET(url);

            var addresses = JsonConvert.DeserializeObject<string[]>(responseContent).Where(x => Regex.IsMatch(x,hexPatter)).ToArray();
       
            return addresses;
        }

        public static async Task<Contract> GetContract(string contractName, string address)
        {
            var contractTask = GetAccount(address);
            var stateTask = GetContractState(address, contractName);

            var account = await contractTask;

            var contract = new Contract(account)
            {
                Name = contractName,
                Methods = new List<string>(),
                Properties = new Dictionary<string, string>()
            };

            var state = await stateTask;
            foreach (var keyValuePair in state)
            {
                if (keyValuePair.Value.Contains("function"))
                {
                    contract.Methods.Add(keyValuePair.Key);
                }
                else
                {
                    contract.Properties.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return contract;
        }

        public static async Task<List<Contract>> GetContractsWithName(string contractName)
        {
            var addresses = await GetContractAddresses(contractName);

            var contractsTask = addresses.Select(async address => await GetContract(contractName, address)).ToList();

            return (await Task.WhenAll(contractsTask)).ToList();
        }

        public static async Task<Dictionary<string, string>> GetContractState(string address, string name)
        {
            var url = ConnectionString.BlocUrl + "/contracts/" + name + "/" + address + "/state";
            var responseContent = await Utils.GET(url);
            var contractState = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            return contractState;
        }
    }

    public class DeployContractModel
    {
        public string password { get; set; }

        public string src { get; set; }
    }

    

}
