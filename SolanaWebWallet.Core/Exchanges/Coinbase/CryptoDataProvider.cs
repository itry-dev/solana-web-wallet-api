using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Exchanges.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges.Coinbase
{
    public class CryptoDataProvider : BaseCryptoDataProvider, ICryptoDataProvider
    {
        public Task<BaseCryptoDataModel> GetCryptoDataBySymbol(string symbol, string exchangeName = null)
        {
            throw new NotImplementedException();
        }
    }
}
