using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Users
{
    public class Account
    {
        //Properties
        public string ContractRoot { get; set; }

        public string Kind { get; set; }

        public string Balance { get; set; }

        public string Address { get; set; }

        public string LatestBlockNum { get; set; }

        public string LatestBlockId { get; set; }

        //Methods
        public bool Send(string address, uint value, string user)
        {
            throw new NotImplementedException();
        }


        //Static Methods
       
    }

}
