using System;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Context;
using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IFileContext _context;

        public FileRepository(IFileContext context)
        {
            _context = context;
        }

        public Task<File> Get(Guid id) => _context.Get(id);

        public Task<File> Create(File file) => _context.Create(file);
    }
}