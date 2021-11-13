using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Interfaces;
using SolanaWebWallet.Core.Managers;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        IWalletManager _walletManager;
        IConfiguration _config;

        public UnitTest1()
        {
            _config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .Build();
        }

        IWalletManager _getWallerManager()
        {
            if (_walletManager != null) return _walletManager;

            var mock = new Mock<ILogger<WalletManager>>();
            var solanaCli = new SolanaCliConfig();
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                _config.GetSection("WinSolanaCli").Bind(solanaCli);
            }
            else
            {
                //2021-11-13 only windows / osx supported at the moment
                _config.GetSection("OSXSolanaCli").Bind(solanaCli);

            }

            _walletManager = new WalletManager(mock.Object, _config, solanaCli);

            return _walletManager;
        }

        [Fact]
        public void TransactionTest()
        {
            var result = _getAddressAtIndex(1);
            Assert.False(string.IsNullOrWhiteSpace(result.address));

            var t = _getWallerManager().TrasanctionTest(result.address).ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.False(string.IsNullOrWhiteSpace(t));
        }

        [Fact]
        public void AddressAtIndexTest()
        {
            var result = _getWallerManager().GetAddressAtIndex(0);

            Assert.False(result.IsFaulted);
        }

        private (string address, string error) _getAddressAtIndex(int index)
        {
            return _getWallerManager().GetAddressAtIndex(index).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
