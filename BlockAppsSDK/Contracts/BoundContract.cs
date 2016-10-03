using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Users;

namespace BlockAppsSDK.Contracts
{
    public class BoundContract<T> : Contract<T>
    {
        public string Username { get; set; }

        public string SigningAddress { get; set; }

        public string SigningPassword { private get; set; }

        public BoundContract(Contract<T> contract) : base(contract as Account)
        {
            Name = contract.Name;
            State = contract.State;
        }

        public void SetSigningAccount(string address, string password)
        {
            SigningAddress = address;
            SigningPassword = password;
        }

        public async Task<string> CallMethod(string methodName, Dictionary<string, string> args, double value)
        {
            return await CallMethod(methodName, args, Username, SigningPassword, SigningAddress, value);
        }
    }
}
