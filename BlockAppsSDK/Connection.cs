using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAppsSDK
{
    public class Connection 
    {

        public string BlocUrl { get; private set; } = "http://localhost:8000/";
        public string StratoUrl { get; private set; } = "http://strato-dev4.blockapps.net/eth/v1.2/";

        public Connection(string blocUrl, string stratoUrl)
        {
            BlocUrl = blocUrl;
            StratoUrl = stratoUrl;
        }
    }
}
