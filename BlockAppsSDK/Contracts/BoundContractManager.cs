using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAppsSDK.Contracts
{
    public class BoundContractManager : ContractManager
    {
        public string Username { get; set; }

        public string Password { private get; set; }

        public string DefaultAddress { get; set; }

        public BoundContractManager(Connection connection)
            : base(connection)
        {
        }

        public async Task<BoundContract> CreateBoundContract(string src, string contractName)
        {
            var boundContract = new BoundContract(await CreateContract(src, contractName, Username, Password, DefaultAddress))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress
            };
            return boundContract;
        }

        public async Task<BoundContract> CreateBoundContract(string src, string contractName, string bindAddress)
        {
            var boundContract = new BoundContract(await CreateContract(src, contractName, Username, Password, bindAddress))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = bindAddress
            };
            return boundContract;
        }

        public async Task<BoundContract> GetBoundContract(string contractName, string address)
        {
            var boundContract = new BoundContract(await base.GetContract(contractName, address))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress
            };
            return boundContract;
        }

        public async Task<BoundContract> GetBoundContract(string contractName, string address, string bindAddress)
        {
            var boundContract = new BoundContract(await base.GetContract(contractName,address))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = bindAddress
            };
            return boundContract;
        }

        public async Task<List<BoundContract>> GetBoundContractsWithName(string contractName)
        {
            var contracts = await GetContractsWithName(contractName);

            if (contracts == null)
            {
                return null;
            }

            var boundContracts = contracts.Select(x => new BoundContract(x)
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress 
            }).ToList();

            return boundContracts;
        }

    }
}
