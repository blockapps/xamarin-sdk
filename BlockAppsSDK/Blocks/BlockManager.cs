using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Blocks
{
    public class BlockManager
    {
        public Connection Connection { get; private set; }

        public BlockManager(Connection connection)
        {
            Connection = connection;
        }

        public async Task<Block> GetBlock(uint blockNumber)
        {
            var url = Connection.StratoUrl + "/block?number=" + blockNumber;
            var a = await Utils.GET(url);
            var blocks = JsonConvert.DeserializeObject<List<Block>>(await Utils.GET(url));
            if (blocks.Count < 1)
            {
                return null;
            }
            return blocks[0];
        }
    }
}
