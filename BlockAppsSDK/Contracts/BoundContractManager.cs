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

        public BoundContractManager(Connection connection)
            : base(connection)
        {
        }

        public async Task<BoundContract> CreateBoundContract(string src, string contractName, string address)
        {
            return new BoundContract(await CreateContract(src, contractName, Username, Password, address));
        }

        public async Task<BoundContract> GetBoundContract(string contractName, string address)
        {
            return new BoundContract(await base.GetContract(contractName,address));
        }

        public async Task<List<BoundContract>> GetBoundContractsWithName(string contractName)
        {
            var contracts = await GetContractsWithName(contractName);

            if (contracts == null)
            {
                return null;
            }

            var boundContracts = contracts.Select(x => new BoundContract(x)).ToList();

            return boundContracts;
        }

    }
}
