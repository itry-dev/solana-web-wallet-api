﻿using System;
using System.Threading.Tasks;

namespace SolanaWebWallet.Core.Interfaces
{
    public interface IWalletManager
    {
        Task<(decimal balance, string error)> GetBalance(string address);

        Task<(string address, string error)> GetMainAddress();

        /// <summary>
        /// Get the qr code of the given text.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Task<byte[]> GetQrCodeAddress(string text);

        /// <summary>
        /// A fake transaction to test you indeed are able to receive sol.
        /// </summary>
        /// <param name="addressFrom"></param>
        /// <returns></returns>
        Task<string> TrasanctionTest();

        /// <summary>
        /// Send the specified amount to the indicated address.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<string> SendTokens(string fromAddress, string toAddress, decimal amount);
    }
}
