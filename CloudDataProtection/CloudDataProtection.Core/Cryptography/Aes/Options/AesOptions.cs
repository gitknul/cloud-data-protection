using System;
using CloudDataProtection.Core.Cryptography.Aes.Constants;

namespace CloudDataProtection.Core.Cryptography.Aes.Options
{
    public class AesOptions
    {
        public string EncryptionKey { get; set; }
        
        public string EncryptionIv { get; set; }

        public int KeySize => AesConstants.KeySize;

        public int BlockSize => AesConstants.BlockSize;

        public byte[] Key => Convert.FromBase64String(EncryptionKey);
        public byte[] Iv => Convert.FromBase64String(EncryptionIv);
    }
}