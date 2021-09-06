using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Solana.WebWallet.Api.Models.Out;
using SolanaWebWallet.Core.Interfaces;

namespace Solana.WebWallet.Api.Controllers
{
    [ApiController]
    [Route("api/v0/[controller]")]
    public class QrCodeController : ControllerBase
    {

        private readonly ILogger<QrCodeController> _logger;
        private readonly IWalletManager _walletManager;

        public QrCodeController(ILogger<QrCodeController> logger, IWalletManager walletManager)
        {
            _logger = logger;
            _walletManager = walletManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<QrCodeModelOut>> Get([FromQuery] string text)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var result = _walletManager.GetQrCodeAddress(text).GetAwaiter().GetResult();
                    return Ok(new QrCodeModelOut { Image = Convert.ToBase64String(result), OriginalText = text });
                });

            }
            catch (Exception e)
            {
                _logger.LogError("Cannot create qrcode for the supplied text", e);
                return StatusCode(500, new ClientErrorData { Title = e.Message });
            }
        }
    }
}
