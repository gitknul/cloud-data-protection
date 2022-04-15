using System;

namespace CloudDataProtection.Core.Cryptography.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptAttribute : Attribute
    {
        /// <summary>
        /// Type of data stored in the column. Only required if the data has to be encrypted during a migration
        /// </summary>
        public DataType DataType { get; set; }
    }

    public enum DataType
    {
        Other = 0,
        EmailAddress = 100
    }
}