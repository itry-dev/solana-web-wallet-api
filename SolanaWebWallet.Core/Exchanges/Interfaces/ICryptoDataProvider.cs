using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolanaWebWallet.Core.Exchanges.Models.Out;

namespace SolanaWebWallet.Core.Exchanges.Interfaces
{
    public interface ICryptoDataProvider
    {
        Task<BaseCryptoDataModel> GetCryptoDataBySymbol(string symbol, string exchangeName=null);
    }
}
