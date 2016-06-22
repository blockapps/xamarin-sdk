using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Users;

namespace BlockAppsSDK.Contracts
{
    public class UserContract : Contract
    {
        public string Username { get; set; }

        public string Password { private get; set; }

        public string DefaultAddress { get; set; }

        public UserContract(Contract contract) : base(contract as Account)
        {
            Name = contract.Name;
            Methods = contract.Methods;
            Properties = contract.Properties;
        }

        public async Task<string> CallMethod(string methodName, Dictionary<string, string> args, double value)
        {
            return await CallMethod(methodName, args, Username, Password, DefaultAddress, value);
        }
        public async Task<string> CallMethod(string methodName, Dictionary<string, string> args, string accountAddress, double value)
        {
            return await CallMethod(methodName, args, Username, Password, accountAddress, value);
        }
    }
}
