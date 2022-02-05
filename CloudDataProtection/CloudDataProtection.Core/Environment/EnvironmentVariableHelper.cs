using System;
using System.Runtime.InteropServices;

namespace CloudDataProtection.Core.Environment
{
    public static class EnvironmentVariableHelper
    {
        public static string GetEnvironmentVariable(string key)
        {
            string environmentVariable = null;
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                environmentVariable = System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process) ?? 
                                      System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ??
                                      System.Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
            }

            if (environmentVariable == null)
            {
                environmentVariable = System.Environment.GetEnvironmentVariable(key);
            }
            
            return environmentVariable;
        }
    }
}