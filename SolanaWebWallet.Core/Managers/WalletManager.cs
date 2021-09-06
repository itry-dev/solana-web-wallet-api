﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using SolanaWebWallet.Core.Interfaces;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing;
using System.IO;

namespace SolanaWebWallet.Core.Managers
{
    public class WalletManager : IWalletManager
    {
        private readonly ILogger<WalletManager> _logger;

        public WalletManager(ILogger<WalletManager> logger)
        {
            _logger = logger;
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

            decimal balance = Convert.ToDecimal(solResponse.response.Replace("SOL", "").Trim());

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
            var solCmd = $"/Users/m20180207/.local/share/solana/install/active_release/bin/{exeName}";

            var stderr = new StringBuilder();
            var stdout = new StringBuilder();

            using Process p = Process.Start(new ProcessStartInfo
            {
                FileName = "zsh",
                Arguments = $"-c \"{solCmd} {command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            _logger.LogDebug($"Executing command -c \"{solCmd} {command}\"");

            while (!p.StandardOutput.EndOfStream)
            {
                string line = p.StandardOutput.ReadLine();
                stdout.AppendLine(line);
                //Console.WriteLine(line);
            }

            while (!p.StandardError.EndOfStream)
            {
                string line = p.StandardError.ReadLine();
                stderr.AppendLine(line);
                //Console.WriteLine(line);
            }

            p.WaitForExit();


            if (p.ExitCode != 0)
            {
                return Task.FromResult((stderr.ToString(), p.ExitCode));
            }

            return Task.FromResult((stdout.ToString(), 0));


        }
        #endregion
    }
}