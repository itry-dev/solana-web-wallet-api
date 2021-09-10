using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Exchanges.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges
{
    public class NoCryptoDataProvider : BaseCryptoDataProvider, ICryptoDataProvider
    {
        readonly Config _config = new Config();
        readonly ILogger _logger;

        public NoCryptoDataProvider(IConfiguration configuration, ILoggerProvider loggerP)
        {
            _config = BindTheProviderConfig<Config>(configuration, "Coingecko");
            _logger = loggerP.CreateLogger("Coingecko:CryptoDataProvider");
        }

        public Task<BaseCryptoDataModel> GetCryptoDataBySymbol(string symbol, string exchangeName= null)
        {
            return Task.Run(() => new BaseCryptoDataModel
            {
                ExchangeName = "NoDataProvider" + (!string.IsNullOrWhiteSpace(exchangeName) ? "("+exchangeName+")" : "")
            });
        }

        #region class Config
        class Config
        {

        }
        #endregion
    }
}
