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

        public async Task<Contract<T>> CreateContract<T>(string src, string contractName, string username, string password, string address)
        {
            var url = Connection.BlocUrl + "/users/" + username + "/" + address + "/contract";
            var postData = "{}";
            postData = new JObject(new JProperty("password", password), new JProperty("src", src)).ToString();
           
            var responseContent = await Utils.POST(url, postData);

            if (responseContent.Contains("invalid"))
            {
                return null;
            }
            return await GetContract<T>(contractName, responseContent);
        }



        public async Task<List<string>> GetContractAddresses(string name)
        {
            var hexPatter = @"^[0-9A-Fa-f]+$";
            var url = Connection.BlocUrl + "/contracts/" + name;
            var responseContent = await Utils.GET(url);

            var addresses = JsonConvert.DeserializeObject<List<string>>(responseContent).Where(x => Regex.IsMatch(x,hexPatter)).ToList();

            return addresses;
        }

        public async Task<Contract<T>> GetContract<T>(string contractName, string address)
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

            var contract = new Contract<T>(contractAccount)
            {
                Name = contractName,
            };

            await contract.RefreshContract();
            
            return contract;
        }

        public async Task<List<Contract<T>>> GetContractsWithName<T>(string contractName)
        {
            //var addresses = await GetContractAddresses(contractName);

            //if (addresses.Length < 1)
            //{
            //    return null;
            //}

            //var contractsTask = addresses.Select(async address => await GetContract<T>(contractName, address)).ToList();
            var url = Connection.BlocUrl + "/search/" + contractName + "/state";
            var responseContent = await Utils.GET(url);

            var dataModels = JsonConvert.DeserializeObject<List<SearchResponseItem<T>>>(responseContent);

            var contracts = dataModels.Select(x => new Contract<T>(new Account(Connection)
            {
                Address = x.Address
            })
            {
                State = x.State
            }).ToList();

            return contracts;

        }

        



    }

    class SearchResponseItem<T>
    {
        public string Address { get; set; }
        public T State { get; set; }
    }
}
