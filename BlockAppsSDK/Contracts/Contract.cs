using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockAppsSDK.Users;

namespace BlockAppsSDK.Contracts
{
    public class Contract : Account
    {
        //Properties
        public string Name { get; set; }

        public List<string> Methods { get; set; }
        
        public Dictionary<string,string> Properties { get; set; }

        //Methods
        public string CallMethod(string name)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentException("name is null or empty", nameof(name));
            }

            throw new NotImplementedException();
        }

        public bool Refresh()
        {

            return true;
        }
    }

    

}
