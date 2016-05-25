using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Users;
using Newtonsoft.Json;

namespace BlockAppsSDK.Contracts
{
    public class Contract : Account
    {
        //Properties
        public string Name { get; set; }

        public List<string> Methods { get; set; }
        
        public Dictionary<string,string> Properties { get; set; }

        //Methods
        public string CallMethod(string name, Dictionary<string,object> args, string password, Account account)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("name is null or empty", nameof(name));
            }

            throw new NotImplementedException();
        }

        public bool Refresh()
        {
            throw new NotImplementedException();
        }

        //Static Methods
        public static async Task<Contract> DeployContract(string src, User user, Account account)
        {
            var url = ConnectionString.BlocUrl + "/users/" + user.Name + "/" + account.Address + "/contract";

            var postModel = new DeployContractModel
            {
                password = user.Password,
                src = src
            };

            var serializedModel = JsonConvert.SerializeObject(postModel);
            var responseContent = await Utils.POST(url, serializedModel);

            throw new NotImplementedException();
        }

        public static Task<string> GetContractAddress(string contractName)
        {
            throw new NotImplementedException();
        }

        public static Task<Contract> GetContract(string contractName)
        {
            throw new NotImplementedException();
        }
    }

    public class DeployContractModel
    {
        public string password { get; set; }

        public string src { get; set; }
    }

    

}
