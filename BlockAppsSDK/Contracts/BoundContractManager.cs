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

        public async Task<BoundContract<T>> CreateBoundContract<T>(string src, string contractName)
        {
            var boundContract = new BoundContract<T>(await CreateContract<T>(src, contractName, Username, Password, DefaultAddress))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress
            };
            return boundContract;
        }

        public async Task<BoundContract<T>> CreateBoundContract<T>(string src, string contractName, string bindAddress)
        {
            var boundContract = new BoundContract<T>(await CreateContract<T>(src, contractName, Username, Password, bindAddress))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = bindAddress
            };
            return boundContract;
        }

        public async Task<BoundContract<T>> GetBoundContract<T>(string contractName, string address)
        {
            var boundContract = new BoundContract<T>(await base.GetContract<T>(contractName, address))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress
            };
            return boundContract;
        }

        public async Task<BoundContract<T>> GetBoundContract<T>(string contractName, string address, string bindAddress)
        {
            var boundContract = new BoundContract<T>(await base.GetContract<T>(contractName,address))
            {
                Username = Username,
                Password = Password,
                DefaultAddress = bindAddress
            };
            return boundContract;
        }

        public async Task<List<BoundContract<T>>> GetBoundContractsWithName<T>(string contractName)
        {
            var contracts = await GetContractsWithName<T>(contractName);

            if (contracts == null)
            {
                return null;
            }

            var boundContracts = contracts.Select(x => new BoundContract<T>(x)
            {
                Username = Username,
                Password = Password,
                DefaultAddress = DefaultAddress 
            }).ToList();

            return boundContracts;
        }

    }
}
