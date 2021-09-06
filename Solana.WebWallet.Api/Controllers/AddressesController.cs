using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Interfaces;

namespace Solana.WebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class AddressesController : ControllerBase
    {

        private readonly ILogger<AddressesController> _logger;
        private readonly IWalletManager _walletManager;

        public AddressesController(ILogger<AddressesController> logger, IWalletManager walletManager)
        {
            _logger = logger;
            _walletManager = walletManager;
        }

        [HttpGet]
        [Route("main")]
        public async Task<ActionResult<string>> Get()
        {
            try
            {
                var result = await _walletManager.GetMainAddress();

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
