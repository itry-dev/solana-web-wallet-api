using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Interfaces;

namespace Solana.WebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class BalanceController : ControllerBase
    {
        
        private readonly ILogger<BalanceController> _logger;
        private readonly IWalletManager _walletManager;

        public BalanceController(ILogger<BalanceController> logger, IWalletManager walletManager)
        {
            _logger = logger;
            _walletManager = walletManager;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string address)
        {
            var result = await _walletManager.GetBalance(address);
            return result.balance.ToString();
        }
    }
}
