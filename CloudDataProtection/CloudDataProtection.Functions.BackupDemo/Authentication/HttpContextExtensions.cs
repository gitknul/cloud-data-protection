using System;
using System.Security.Cryptography;
using System.Text;
using CloudDataProtection.Core.Environment;
using Microsoft.AspNetCore.Http;

namespace CloudDataProtection.Functions.BackupDemo.Authentication
{
    public static class HttpContextExtensions
    {
        private static readonly string AuthIndex = "x-functions-key";

        private static readonly string ApiKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_API_KEY");
        
        public static bool IsAuthenticated(this HttpContext context)
        {
            string token = context.Request.Headers[AuthIndex];

            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            using (SHA512 sha = SHA512.Create())
            {
                byte[] hashedInput = sha.ComputeHash(Encoding.UTF8.GetBytes(token));

                string hashedInputString = Convert.ToBase64String(hashedInput);

                return ApiKey == hashedInputString;
            }
        }
    }
}