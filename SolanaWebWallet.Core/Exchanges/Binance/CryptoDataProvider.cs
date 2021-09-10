using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Exchanges.Binance;
using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Exchanges.Models.Out;

namespace SolanaWebWallet.Core.Exchanges.Binance
{
    public class CryptoDataProvider : BaseCryptoDataProvider, ICryptoDataProvider
    {
        readonly Config _config = new Config();
        readonly ILogger _logger;
        readonly ILoggerProvider _loggerProvider;
        readonly IConfiguration _configuration;

        public CryptoDataProvider(IConfiguration configuration, ILoggerProvider loggerP)
        {
            _configuration = configuration;
            _config = BindTheProviderConfig<Config>(configuration, "Binance");
            _logger = loggerP.CreateLogger("Binance:CryptoDataProvider");
            _loggerProvider = loggerP;
        }

        #region Config class
        class Config
        {
            public string ApiUrl { get; set; }

            public string CryptoDataInfoUrl { get; set; }
        } 
        #endregion

        #region GetCryptoDataBySymbol
        public async Task<BaseCryptoDataModel> GetCryptoDataBySymbol(string symbol, string exchangeName = null)
        {
            //TODO implementare binance
            return await GetDefaultProvider(_configuration, _loggerProvider).GetCryptoDataBySymbol(symbol, "Binance");
        }
        #endregion

    }
}
