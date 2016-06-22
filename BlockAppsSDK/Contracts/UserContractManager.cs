using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAppsSDK.Contracts
{
    public class UserContractManager : ContractManager
    {
        public string Username { get; set; }

        public string Password { private get; set; }

        public UserContractManager(Connection connection)
            : base(connection)
        {
        }

        public async Task<UserContract> CreateUserContract(string src, string contractName, string address)
        {
            return new UserContract(await CreateContract(src, contractName, Username, Password, address));
        }

        public async Task<UserContract> GetUserContract(string contractName, string address)
        {
            return new UserContract(await base.GetContract(contractName,address));
        }
    }
}
