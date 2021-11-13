using System;
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
    public class AddressesController : BaseController
    {

        private readonly ILogger<AddressesController> _logger;

        public AddressesController(ILogger<AddressesController> logger, IWalletManager walletManager, IOptions<SolanaCliConfig> solanaCliConfig) : base(solanaCliConfig, walletManager)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the wallet main address.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("main")]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                var result = await WallterManager.GetMainAddress();

                if (!string.IsNullOrWhiteSpace(result.error))
                {
                    return BadRequest(new ClientErrorData { Title = result.error });
                }

                return result.address;
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot get main wallet address", e);
                return StatusCode(500, new ClientErrorData { Title = e.Message });
            }
        }
    }
}
