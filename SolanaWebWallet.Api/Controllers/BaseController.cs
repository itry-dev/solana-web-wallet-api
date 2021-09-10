using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolanaWebWallet.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly SolanaCliConfig SolanaCliConfiguration;

        protected readonly IWalletManager WallterManager;

        public BaseController(IOptions<SolanaCliConfig> solanaCliConfig, IWalletManager walletManager)
        {
            SolanaCliConfiguration = solanaCliConfig.Value;
            WallterManager = walletManager;
        }
    }
}
