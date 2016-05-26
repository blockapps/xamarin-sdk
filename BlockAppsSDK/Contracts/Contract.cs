using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Users;
using Newtonsoft.Json;

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
        public string CallMethod(string name, Dictionary<string,object> args, string password, Account account)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("name is null or empty", nameof(name));
            }

            throw new NotImplementedException();
        }

        public bool Refresh()
        {
            throw new NotImplementedException();
        }

        //Static Methods
        //private static string parseContractName(string src)
        //{
        //    var last = src.IndexOf('{');
        //}

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
            var address = JsonConvert.DeserializeObject<string>(responseContent);
            return await GetContract(contractName);

            
        }

        public static async Task<string> GetContractAddress(string name)
        {
            var url = ConnectionString.BlocUrl + "/contracts/" + name;
            var responseContent = await Utils.GET(url);
            var contractState = JsonConvert.DeserializeObject<string[]>(responseContent)[0];

            return contractState; ;
        }

        public static async Task<Contract> GetContract(string contractName)
        {
            var address = await GetContractAddress(contractName);

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
