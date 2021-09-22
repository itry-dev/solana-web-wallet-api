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

            return new WalletManager(mock.Object, _config);
        }

        [Fact]
        public void TransactionTest()
        {
            var result = _getWallerManager().TrasanctionTest();

            Assert.True(result.IsFaulted);
        }
    }
}
