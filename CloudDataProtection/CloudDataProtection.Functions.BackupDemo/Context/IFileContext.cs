using System;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Context
{
    public interface IFileContext
    {
        Task<File> Get(Guid id);
        Task<File> Create(File file);
    }
}