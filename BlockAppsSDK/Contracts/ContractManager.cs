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
    public class ContractManager : AccountManager
    {

        public ContractManager(Connection connection) 
            :base(connection)
        {
        }

        public async Task<Contract> CreateContract(string src, string contractName, string username, string password, string address)
        {
            var url = Connection.BlocUrl + "/users/" + username + "/" + address + "/contract";
            var postData = "{}";

            postData = new JObject(new JProperty("password", password), new JProperty("src", src)).ToString();
            //var postModel = new DeployContractModel
            //{
            //    password = user.Password,
            //    src = src
            //};
            //var serializedModel = JsonConvert.SerializeObject(postData);
            var responseContent = await Utils.POST(url, postData);

            if (responseContent.Contains("invalid"))
            {
                return null;
            }
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
            var contractAccountTask = GetAccount(address);
            var contractAddressesTask = GetContractAddresses(contractName);
            //var stateTask = GetContractState(address, contractName);

            var addresses = await contractAddressesTask;
            if (!addresses.Contains(address))
            {
                return null;
            }

            var contractAccount = await contractAccountTask;

            var contract = new Contract(contractAccount)
            {
                Name = contractName,
                Methods = new List<string>(),
                Properties = new Dictionary<string, string>()
            };

            await contract.RefreshContract();
            
            return contract;
        }

        public async Task<List<Contract>> GetContractsWithName(string contractName)
        {
            var addresses = await GetContractAddresses(contractName);

            if (addresses.Length < 1)
            {
                return null;
            }

            var contractsTask = addresses.Select(async address => await GetContract(contractName, address)).ToList();

            return (await Task.WhenAll(contractsTask)).ToList();
        }
        

    }
}
