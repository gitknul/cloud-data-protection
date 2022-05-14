using System;
using System.Security.Cryptography;
using System.Text;
using CloudDataProtection.Core.Cryptography.Aes.Constants;

namespace CloudDataProtection.CI.KeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating AES key and IV with the following settings:\n");

            using (Aes aes = CreateAes())
            {
                Console.WriteLine($"Key size: {aes.KeySize}");
                Console.WriteLine($"Block size: {aes.BlockSize}");

                Console.WriteLine();
            
                aes.GenerateKey();
                aes.GenerateIV();

                string key = Convert.ToBase64String(aes.Key);
                string iv = Convert.ToBase64String(aes.IV);

                Console.WriteLine($"Key: {key}");
                Console.WriteLine($"Iv: {iv}\n");
            }

            using (SHA512 sha = SHA512.Create())
            {
                Console.WriteLine("Generating API key:\n");
            
                string key = Guid.NewGuid().ToString();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(key));

                Console.WriteLine($"Key: {key}");
                Console.WriteLine($"Hashed key: {Convert.ToBase64String(hash)}");
            }
        }

        private static Aes CreateAes()
        {
            return new AesManaged
            {
                KeySize = AesConstants.KeySize,
                BlockSize = AesConstants.BlockSize
            };
        }
    }
}