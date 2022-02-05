using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Repository;
using CloudDataProtection.Functions.BackupDemo.Service;
using CloudDataProtection.Functions.BackupDemo.Service.Result;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Result;
using Microsoft.AspNetCore.Http;
using File = CloudDataProtection.Functions.BackupDemo.Entities.File;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileManagerLogic
    {
        private readonly IDataTransformer _transformer;
        private readonly IFileRepository _repository;
        private readonly IEnumerable<IFileService> _fileServices;

        public IEnumerable<IFileService> FileServices => _fileServices.ToImmutableList();

        public FileManagerLogic(IEnumerable<IFileService> fileServices, 
            IDataTransformer transformer,
            IFileRepository repository)
        {
            _fileServices = fileServices;
            _transformer = transformer;
            _repository = repository;
        }

        public async Task<BusinessResult<File>> Upload(IFormFile input, ICollection<FileDestination> destinations)
        {
            string fileName = GenerateFileName();
            
            using (Stream stream = _transformer.Encrypt(input.OpenReadStream()))
            {
                File file = new()
                {
                    ContentType = input.ContentType,
                    DisplayName = input.FileName,
                    Bytes = input.Length
                };
                
                foreach (FileDestination destination in destinations)
                {
                    IFileService fileService = ResolveFileService(destination);

                    if (fileService == null)
                    {
                        continue;
                    }

                    FileDestinationInfo info = new(destination);

                    try
                    {
                        MemoryStream copy = new();

                        await stream.CopyToAndSeekAsync(copy);

                        UploadFileResult fileResult = await fileService.Upload(copy, fileName);

                        info.UploadCompletedAt = DateTime.Now;
                        info.UploadSuccess = fileResult.Success;
                        info.FileId = fileResult.Id;
                    }
                    catch (Exception e)
                    {
                        info.UploadSuccess = false;
                        info.FileId = null;
                    }

                    file.AddDestination(info);
                }

                if (file.IsUploaded)
                {
                    await _repository.Create(file);
                }

                return BusinessResult<File>.Ok(file);
            }      
        }

        public async Task<BusinessResult<File>> Get(Guid id)
        {
            File file = await _repository.Get(id);

            if (file == null)
            {
                return BusinessResult<File>.NotFound($"Could not find file with id = {id.ToString()}");
            }
            
            return BusinessResult<File>.Ok(file);
        }

        public async Task<BusinessResult<FileDownloadResult>> Download(Guid id)
        {
            BusinessResult<File> result = await Get(id);
            
            if (!result.Success)
            {
                return BusinessResult<FileDownloadResult>.Error("An unknown error occured while retrieving info of the file");
            }
            
            return await Download(result.Data);
        }

        private async Task<BusinessResult<FileDownloadResult>> Download(File file)
        {
            bool downloaded = false;
            byte[] data = Array.Empty<byte>();
            FileDestination? downloadedFrom = null;

            for (int i = 0; i < file.UploadedTo.Count && !downloaded; i++)
            {
                FileDestinationInfo info = file.UploadedTo[i];

                try
                {
                    IFileService fileService = ResolveFileService(info.Destination);

                    if (fileService == null)
                    {
                        continue;
                    }
                    
                    Stream response = await fileService.GetDownloadStream(info.FileId);
                    
                    data = _transformer.Decrypt(response);
                }
                catch (Exception e)
                {
                    continue;
                }

                downloaded = data != null && data.Length > 0;

                if (downloaded)
                {
                    downloadedFrom = info.Destination;
                }
            }

            if (!downloaded)
            {
                return BusinessResult<FileDownloadResult>.Error("An unknown error occured while attempting to download the file");
            }
            
            FileDownloadResult result = new()
            {
                Bytes = data,
                FileName = file.DisplayName,
                ContentType = file.ContentType,
                DownloadedFrom = downloadedFrom
            };
            
            return BusinessResult<FileDownloadResult>.Ok(result);
        }

        private string GenerateFileName()
        {
            return Path.GetRandomFileName().Split('.')[0];
        }

        private IFileService ResolveFileService(FileDestination destination)
        {
            return _fileServices.FirstOrDefault(fs => fs.Destination == destination);
        }
    }
}