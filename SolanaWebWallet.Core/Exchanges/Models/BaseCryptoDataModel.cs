using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges.Models.Out
{
    public class BaseCryptoDataModel
    {
        public virtual string Id { get; set; }

        public virtual string Symbol { get; set; }

        public virtual string ExchangeName { get; set; }

        public BaseMarket[] Market { get; set; }
    }

    public class BaseMarket
    {
        public virtual string Name { get; set; }

        public virtual decimal Last { get; set; }

        public virtual string Target { get; set; }

        public BaseCryptoImage Image { get; set; }
    }

    public class BaseCryptoImage
    {
        public virtual string Thumbnail { get; set; }

        public virtual string Large { get; set; }

        public virtual string Small { get; set; }
    }
}
