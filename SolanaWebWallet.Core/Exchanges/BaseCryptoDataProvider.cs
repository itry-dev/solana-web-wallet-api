using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Exchanges
{
    public class BaseCryptoDataProvider
    {
        protected Coingecko.CryptoDataProvider GetDefaultProvider(IConfiguration config, ILoggerProvider loggerProvider)
        {
            return new Coingecko.CryptoDataProvider(config, loggerProvider);
        }

        protected T BindTheProviderConfig<T>(IConfiguration configuration, string sectionName) where T : class
        {
            var section = configuration.GetSection("CryptoProviders");
            if (!section.Exists()) throw new Exception("Section CryptoProviders does not exists");

            var children = section.GetChildren();
            if (!children.Any()) throw new Exception("Section CryptoProviders does not have children");

            T obj = default(T);

            var list = children.ToList();

            foreach (var item in list)
            {
                var subSection = item.GetSection($"{sectionName}");
                if (subSection.Exists())
                {
                    obj = Activator.CreateInstance<T>();
                    subSection.Bind(obj);
                    break;
                }
            }

            return obj ?? throw new Exception($"{sectionName} not found");
        }
    }
}
