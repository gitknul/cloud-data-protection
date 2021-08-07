using System;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Repository
{
    public interface IFileRepository
    {
        Task<File> Get(Guid id);
        Task<File> Create(File file);
    }
}