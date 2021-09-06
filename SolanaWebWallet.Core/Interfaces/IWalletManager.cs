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
    }
}
