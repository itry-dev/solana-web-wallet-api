using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges.Interfaces
{
    public interface ICryptoProviderFactory
    {
        ICryptoDataProvider CreateProvider(string providerName);
    }
}
