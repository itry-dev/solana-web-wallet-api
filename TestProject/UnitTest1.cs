using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
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

            _walletManager = new WalletManager(mock.Object, _config);

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
