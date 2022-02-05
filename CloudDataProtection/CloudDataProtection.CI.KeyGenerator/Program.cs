using System;
using System.Security.Cryptography;
using CloudDataProtection.Core.Cryptography.Aes.Constants;

namespace CloudDataProtection.CI.KeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating AES key and IV with the following settings:\n");
            
            Aes aes = new AesManaged
            {
                KeySize = AesConstants.KeySize,
                BlockSize = AesConstants.BlockSize
            };
            
            Console.WriteLine($"Key size: {aes.KeySize}");
            Console.WriteLine($"Block size: {aes.BlockSize}");

            Console.Write("\n");
            
            aes.GenerateKey();
            aes.GenerateIV();

            string key = Convert.ToBase64String(aes.Key);
            string iv = Convert.ToBase64String(aes.IV);

            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Iv: {iv}\n");

            Console.WriteLine("Generating API key:\n");
            
            aes.GenerateKey();

            string apiKey = Convert.ToBase64String(aes.Key);
            
            Console.WriteLine($"Key: {apiKey}");
        }
    }
}