using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using SolanaWebWallet.Core.Interfaces;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using SolanaWebWallet.Core.Configuration;

namespace SolanaWebWallet.Core.Managers
{
    public class WalletManager : IWalletManager
    {
        private readonly ILogger<WalletManager> _logger;
        private readonly IConfiguration _configuration;
        private readonly SolanaCliConfig _solanaCliConfig = new SolanaCliConfig();

        public WalletManager(ILogger<WalletManager> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            configuration.GetSection("SolanaCli").Bind(_solanaCliConfig);
        }

        #region GetBalance
        public Task<(decimal balance, string error)> GetBalance(string address)
        {
            var wResult = GetMainAddress().GetAwaiter().GetResult();
            if (!string.IsNullOrWhiteSpace(wResult.error))
            {
                return Task.FromException<(decimal, string)>(new Exception(wResult.error));
            }

            var solResponse = _getSolanaResponse("solana", "balance " + wResult.address).GetAwaiter().GetResult();

            if (solResponse.processCode != 0)
            {
                return Task.FromException<(decimal, string)>(new Exception($"Error code {solResponse.processCode}"));
            }

            _logger.LogDebug(solResponse.response);

            decimal balance = Convert.ToDecimal(
                                solResponse.response.Replace("SOL", "")
                                .Replace(Environment.NewLine,"")
                                .Trim()
                              , new System.Globalization.CultureInfo("en-US"));

            return Task.FromResult( (balance, "") );
        }
        #endregion

        #region GetMainAddress
        public Task<(string address, string error)> GetMainAddress()
        {
            var solResponse = _getSolanaResponse("solana-keygen", "pubkey usb://ledger").GetAwaiter();

            var solResult = solResponse.GetResult();

            if (solResult.processCode != 0)
            {
                return Task.FromException<(string,string)>( new Exception(solResult.response) );
            }

            return Task.FromResult( (solResult.response,String.Empty) );
        }
        #endregion

        #region GetQrCodeAddress
        public Task<byte[]> GetQrCodeAddress(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            return Task.FromResult( _imageToByte(qrCodeImage) );
        }

        private byte[] _imageToByte(Image img)
        {
            using var stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
        #endregion

        #region _getSolanaResponse
        private Task<(string response, int processCode)> _getSolanaResponse(string exeName, string command)
        {
            var fi = new FileInfo(_solanaCliConfig.SolanaHome);
            var solCmd =  $"{exeName}";

            var stderr = new StringBuilder();
            var stdout = new StringBuilder();

            using Process p = Process.Start(new ProcessStartInfo
            {
                FileName = _solanaCliConfig.OpenWith,
                WorkingDirectory = fi.DirectoryName,
                Arguments = $"/c {solCmd} {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            _logger.LogDebug($"Executing command /c {solCmd} {command}");

            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                stdout.AppendLine(line);
            }

            while (!p.StandardError.EndOfStream)
            {
                string line = p.StandardError.ReadLine();
                stderr.AppendLine(line);
            }

            p.WaitForExit();


            if (p.ExitCode != 0)
            {
                return Task.FromResult((stderr.ToString(), p.ExitCode));
            }

            return Task.FromResult((stdout.ToString(), 0));


        }
        #endregion

        #region TrasanctionTest
        public Task<string> TrasanctionTest()
        {
            #region create a temp address
            Task<(string response, int processCode)> solResponse = _getSolanaResponse("solana-keygen", "new --no-passphrase --no-outfile");
            solResponse.Wait();

            if (solResponse.IsFaulted)
            {
                return Task.FromException<string>(new Exception(solResponse.Exception.Message));
            }
            else if (solResponse.Result.processCode != 0)
            {
                return Task.FromException<string>(new Exception($"Error code {solResponse.Result.response}"));
            }
            #endregion

            //l'indirizzo creato
            string pubKey = "";
            var match = System.Text.RegularExpressions.Regex.Match(solResponse.Result.response, "pubkey:\\s[^=]+");
            if (match.Success)
            {
                pubKey = match.Value.Replace("pubkey: ", "").Trim();
            }
            //string pubTempKey = System.IO.File.ReadAllText(tmpPath).Replace("pubkey:", String.Empty).Trim();

            #region airdrop to the newly created address
            solResponse = _getSolanaResponse("solana", $"airdrop 1 {pubKey} --url {_solanaCliConfig.AirdropTestUrl}");
            solResponse.Wait();

            if (solResponse.IsFaulted)
            {
                return Task.FromException<string>(new Exception(solResponse.Exception.Message));
            }
            else if (solResponse.Result.processCode != 0)
            {
                return Task.FromException<string>(new Exception($"Error code {solResponse.Result.response}"));
            }

            //airdrop result
            _logger.LogDebug(solResponse.Result.response);
            #endregion

            #region checking sol balance
            solResponse = _getSolanaResponse("solana", $"balance {pubKey} --url {_solanaCliConfig.AirdropTestUrl}");
            solResponse.Wait();

            if (solResponse.IsFaulted)
            {
                return Task.FromException<string>(new Exception(solResponse.Exception.Message));
            }
            else if (solResponse.Result.processCode != 0)
            {
                return Task.FromException<string>(new Exception($"Error code {solResponse.Result.response}"));
            }
            //balance result
            _logger.LogDebug(solResponse.Result.response); 
            #endregion

            return Task.FromResult(solResponse.Result.response.Replace("SOL", "").Trim());
        }
        #endregion

        #region SendTokens
        public Task<string> SendTokens(string fromAddress, string toAddress, decimal amount)
        {
            Task<(string response, int processCode)> solResponse = _getSolanaResponse("solana", $"transfer --from {fromAddress} {toAddress} {amount} --fee-payer {fromAddress}");
            solResponse.Wait();

            if (solResponse.IsFaulted)
            {
                return Task.FromException<string>(new Exception(solResponse.Exception.Message));
            }
            else if (solResponse.Result.processCode != 0)
            {
                return Task.FromException<string>(new Exception($"Error code {solResponse.Result.response}"));
            }

            return Task.FromResult(solResponse.Result.response);
        }
        #endregion
    }
}
