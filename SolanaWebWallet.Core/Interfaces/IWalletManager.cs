using System;
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
        /// A fake transaction to test you indeed hold the corresponding private key. It returns the number of sol getted, usually 1.
        /// </summary>
        /// <param name="pubKey"></param>
        /// <returns></returns>
        Task<string> TrasanctionTest(string pubKey);

        /// <summary>
        /// Send the specified amount to the indicated address.
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<string> SendTokens(string fromAddress, string toAddress, decimal amount);

        /// <summary>
        /// Get the address at specified index or error if something went wrong.
        /// </summary>
        /// <returns></returns>
        Task<(string address, string error)> GetAddressAtIndex(int index);
    }
}
