using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Exchanges.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using signalr = Microsoft.AspNetCore.SignalR;

namespace SolanaWebWallet.Api.Hub
{
    public class QuotationHub : signalr.Hub
    {
        readonly ILogger<QuotationHub> _logger;

        public QuotationHub(ILogger<QuotationHub> logger)
        {
            _logger = logger;
        }

        public async Task PingQuotation(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return;

            await Clients.All.SendCoreAsync("PongQuotation", new object[] { "["+DateTime.Now.ToString()+"]" + source + " vale mille mila euri" });
        }
    }
}
