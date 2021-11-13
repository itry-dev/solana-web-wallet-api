using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Interfaces;

namespace SolanaWebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class BalanceController : BaseController
    {
        
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(ILogger<BalanceController> logger, IWalletManager walletManager, IOptions<SolanaCliConfig> solanaCliConfig) : base(solanaCliConfig, walletManager)
        {
            _logger = logger;
        }

        /// <summary>
        /// Balance of the main address wallet.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Get([FromQuery] string address)
        {
            var result = await WallterManager.GetBalance(address);
            return result.balance.ToString(new System.Globalization.CultureInfo("en-US"));
        }
    }
}
