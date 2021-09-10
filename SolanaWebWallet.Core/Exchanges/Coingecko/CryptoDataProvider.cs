using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Exchanges.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges.Coingecko
{
    public class CryptoDataProvider : BaseCryptoDataProvider, ICryptoDataProvider
    {
        readonly Config _config = new Config();
        readonly ILogger _logger;

        public CryptoDataProvider(IConfiguration configuration, ILoggerProvider loggerP)
        {
            _config = BindTheProviderConfig<Config>(configuration, "Coingecko");
            _logger = loggerP.CreateLogger("Coingecko:CryptoDataProvider");
        }

        #region Config class
        public class Config
        {
            public string ApiUrl { get; set; }

            public string CryptoDataInfoUrl { get; set; }
        }
        #endregion

        #region GetCryptoDataBySymbol
        public async Task<BaseCryptoDataModel> GetCryptoDataBySymbol(string symbol, string exchangeName = null)
        {
            var coinUrl = $"{_config.ApiUrl}/{_config.CryptoDataInfoUrl.Replace("{id}", symbol)}";
            if (!string.IsNullOrWhiteSpace(exchangeName))
            {
                coinUrl += $"/tickers?exchange_ids={exchangeName}";
            }
            var response = await new HttpClient().GetAsync(coinUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

            var jResponse = await response.Content.ReadAsStringAsync();

            var container = Newtonsoft.Json.JsonConvert.DeserializeObject<TickerContainer>(jResponse);

            var model = new BaseCryptoDataModel
            {
                Symbol = container.Name,
                ExchangeName = exchangeName ?? container.Tickers.Where(w => w.BaseName == exchangeName).Select(s => s.BaseName).FirstOrDefault()
            };

            var marketData = new List<BaseMarket>();
            foreach (var ticker in container.Tickers)
            {
                marketData.Add(new BaseMarket
                {
                    Name = ticker.BaseName,
                    Target = ticker.Target,
                    Last = ticker.Last
                });
            }

            model.Market = marketData.ToArray();

            return model;
        } 
        #endregion
    }
}
