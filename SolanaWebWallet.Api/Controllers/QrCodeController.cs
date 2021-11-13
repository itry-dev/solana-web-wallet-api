using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solana.WebWallet.Api.Models.Out;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Interfaces;

namespace SolanaWebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class QrCodeController : BaseController
    {

        private readonly ILogger<QrCodeController> _logger;

        public QrCodeController(ILogger<QrCodeController> logger, IWalletManager walletManager, IOptions<SolanaCliConfig> solanaCliConfig) : base(solanaCliConfig, walletManager)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the QR Code of the main wallet address.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<QrCodeModelOut>> Get([FromQuery] string text)
        {
            try
            {
                var result = await WallterManager.GetQrCodeAddress(text);
                return Ok(new QrCodeModelOut { Image = Convert.ToBase64String(result), OriginalText = text });
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot create qrcode for the supplied text", e);
                return StatusCode(500, new ClientErrorData { Title = e.Message });
            }
        }
    }
}
