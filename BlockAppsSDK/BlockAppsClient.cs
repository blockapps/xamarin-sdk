using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BlockAppsSDK
{
    public class BlockAppsClient
    {
        public Connection Connection { get; set; }

        public BlockAppsClient(string blocUrl, string stratoUrl)
            : this(new Connection(blocUrl,stratoUrl))
        {
        }

        public BlockAppsClient(Connection connection)
        {
            Connection = connection;
        }


    }

    
}
