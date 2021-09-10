using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Exchanges.Interfaces;

namespace SolanaWebWallet.Core.Exchanges
{
    public class CryptoProviderFactory : ICryptoProviderFactory
    {
        readonly IConfiguration _configuration;
        readonly ILoggerProvider _loggerP;
        readonly ILogger _logger;

        public CryptoProviderFactory(IConfiguration configuration, ILoggerProvider loggerP)
        {
            _configuration = configuration;
            _loggerP = loggerP;
            _logger = _loggerP.CreateLogger(nameof(CryptoProviderFactory));
        }

        public ICryptoDataProvider CreateProvider(string providerName)
        {
            try
            {
                return (ICryptoDataProvider)Activator.CreateInstance(Type.GetType($"SolanaWebWallet.Core.Exchanges.{providerName}.CryptoDataProvider"), _configuration, _loggerP);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot create exchange with a name of {providerName}");

                return (ICryptoDataProvider)Activator.CreateInstance(Type.GetType("SolanaWebWallet.Core.Exchanges.NoCryptoDataProvider"), _configuration, _loggerP);
            }
            
        }
    }
}
