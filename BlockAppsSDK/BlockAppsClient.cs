using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlockAppsSDK.Blocks;
using BlockAppsSDK.Contracts;
using BlockAppsSDK.Users;
using Xamarin.Forms;

namespace BlockAppsSDK
{
    public class BlockAppsClient
    {
        public Connection Connection { get; set; }

        public UserManager UserManager { get; private set; }
        public BlockManager BlockManager { get; private set; }


        public BlockAppsClient(string blocUrl, string stratoUrl)
            : this(new Connection(blocUrl, stratoUrl))
        {
        }

        public BlockAppsClient(Connection connection)
        {

            Connection = connection;
            UserManager = new UserManager(connection);
            BlockManager = new BlockManager(connection);
        }
    }


}
