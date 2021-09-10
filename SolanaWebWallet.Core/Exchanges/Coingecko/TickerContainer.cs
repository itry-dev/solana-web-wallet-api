using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges.Coingecko
{
    public class TickerContainer
    {
        public string Name { get; set; }

        public Ticker[] Tickers { get; set; }
    }

    public class Ticker
    {
        [JsonProperty("base")]
        public string BaseName { get; set; }

        public string Target { get; set; }

        public decimal Last { get; set; }
    }
}
