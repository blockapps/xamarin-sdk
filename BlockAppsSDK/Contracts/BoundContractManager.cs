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

        public string SigningPassword { private get; set; }

        public string SigningAddress { get; set; }


        public BoundContractManager(Connection connection)
            : base(connection)
        {
        }

        public async Task<BoundContract<T>> CreateBoundContract<T>(string src, string contractName)
        {
            var boundContract = new BoundContract<T>(await CreateContract<T>(src, contractName, Username, SigningPassword, SigningAddress))
            {
                Username = Username,
                SigningPassword = SigningPassword,
                SigningAddress = SigningAddress
            };
            return boundContract;
        }

        public async Task<BoundContract<T>> GetBoundContract<T>(string contractName, string address)
        {
            var boundContract = new BoundContract<T>(await base.GetContract<T>(contractName, address))
            {
                Username = Username,
                SigningPassword = SigningPassword,
                SigningAddress = SigningAddress
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
                Password = SigningPassword,
                SigningAddress = SigningAddress 
            }).ToList();

            return boundContracts;
        }

    }
}
