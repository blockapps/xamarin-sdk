using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlockAppsSDK.Blocks
{
    public class Block
    {
        public string Next { get; set; }
        public string Kind { get; set; }
        public string Hash { get; set; }
        public List<string> BlockUncles { get; set; }
        public List<ReceiptTransaction> ReceiptTransactions { get; set; }
        public BlockData BlockData { get; set; }

        
    }

    public class ReceiptTransaction : Transaction
    {
        public string TransactionType { get; set; }
        public string Kind { get; set; }
        public string Init { get; set; }
    }

    public class BlockData
    {
        public int ExtraData { get; set; }
        public int GasUsed { get; set; }
        public int GasLimit { get; set; }
        public string Kind { get; set; }
        public string UnclesHash { get; set; }
        public string MixHash { get; set; }
        public string ReceiptsRoot { get; set; }
        public int Number { get; set; }
        public int Difficulty { get; set; }
        public string Timestamp { get; set; }
        public string Coinbase { get; set; }
        public string ParentHash { get; set; }
        public int Nonce { get; set; }
        public string StateRoot { get; set; }
        public string TransactionsRoot { get; set; }
    }

    public class Transaction
    {
        public string Hash { get; set; }
        public int GasLimit { get; set; }
        public string Data { get; set; }
        public int GasPrice { get; set; }
        public string CodeOrData { get; set; }
        public string To { get; set; }
        public int Value { get; set; }
        public string From { get; set; }
        public string r { get; set; }
        public string s { get; set; }
        public string v { get; set; }
        public int Nonce { get; set; }
    }

    

}
