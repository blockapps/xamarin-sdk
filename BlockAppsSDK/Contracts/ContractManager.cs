using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlockAppsSDK.Users;
using Newtonsoft.Json;

namespace BlockAppsSDK.Contracts
{
    public class ContractManager : AccountManager
    {

        public ContractManager(Connection connection) 
            :base(connection)
        {
        }

        public async Task<Contract> DeployContract(string src, string contractName, User user, Account account)
        {
            var url = Connection.BlocUrl + "/users/" + user.Name + "/" + account.Address + "/contract";

            var postModel = new DeployContractModel
            {
                password = user.Password,
                src = src
            };
            var serializedModel = JsonConvert.SerializeObject(postModel);
            var responseContent = await Utils.POST(url, serializedModel);
            return await GetContract(contractName, responseContent);

            
        }

        public async Task<string[]> GetContractAddresses(string name)
        {
            var hexPatter = @"^[0-9A-Fa-f]+$";
            var url = Connection.BlocUrl + "/contracts/" + name;
            var responseContent = await Utils.GET(url);

            var addresses = JsonConvert.DeserializeObject<string[]>(responseContent).Where(x => Regex.IsMatch(x,hexPatter)).ToArray();
       
            return addresses;
        }

        public async Task<Contract> GetContract(string contractName, string address)
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
        public async Task<List<Contract>> GetContractsWithName(string contractName)
        {
            var addresses = await GetContractAddresses(contractName);

            var contractsTask = addresses.Select(async address => await GetContract(contractName, address)).ToList();

            return (await Task.WhenAll(contractsTask)).ToList();
        }

        public async Task<Dictionary<string, string>> GetContractState(string address, string name)
        {
            var url = Connection.BlocUrl + "/contracts/" + name + "/" + address + "/state";
            var responseContent = await Utils.GET(url);
            var contractState = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);

            return contractState;
        }
    }
}
