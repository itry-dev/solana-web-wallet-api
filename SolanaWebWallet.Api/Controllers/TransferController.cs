using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace SolanaWebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class TransferController : BaseController
    {
        private readonly ILogger<QrCodeController> _logger;

        public TransferController(ILogger<QrCodeController> logger, IWalletManager walletManager, IOptions<SolanaCliConfig> solanaCliConfig) : base(solanaCliConfig, walletManager)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("TransactionTest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> TransactionTest([FromQuery] string pubKey)
        {
            try
            {
                var result = await WallterManager.TrasanctionTest(pubKey);
                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError("Transaction testa failed", e);
                return StatusCode(500, new ClientErrorData { Title = e.Message });
            }
        }
    }
}
