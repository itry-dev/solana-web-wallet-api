using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Exchanges.Models.Out;
using SolanaWebWallet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolanaWebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly ILogger<ExchangeController> _logger;
        private readonly ICryptoProviderFactory _dpFactory;


        public ExchangeController(ILogger<ExchangeController> logger, ICryptoProviderFactory dpFactory)
        {
            _logger = logger;
            _dpFactory = dpFactory;
        }

        /// <summary>
        /// Using CoinGecko API get the quotation from Coinbase, Binance and KuCoin exchagens.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="exchanges"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<BaseCryptoDataModel>>> GetCryptoData([FromQuery] string symbol, [FromQuery] string exchanges)
        {
            if (string.IsNullOrWhiteSpace(exchanges)) return BadRequest(new ClientErrorData { Title = "Exchanges cannot be empy" });
            var tokens = exchanges.Split(new char[] { ',' });

            var models = new List<BaseCryptoDataModel>();

            foreach (var exchange in tokens)
            {
                var model = await _dpFactory.CreateProvider("Coingecko").GetCryptoDataBySymbol(symbol, exchange);
                _logger.LogInformation($"retreiving data from exchange {exchange}");
                models.Add(model);
            }

            return Ok(models);
        }
    }
}
