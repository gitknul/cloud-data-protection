using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Repository;
using CloudDataProtection.Functions.BackupDemo.Service;
using CloudDataProtection.Functions.BackupDemo.Service.Amazon;
using CloudDataProtection.Functions.BackupDemo.Service.Google;
using CloudDataProtection.Functions.BackupDemo.Service.Result;
using CloudDataProtection.Functions.BackupDemo.Triggers.Dto;
using Microsoft.AspNetCore.Http;
using File = CloudDataProtection.Functions.BackupDemo.Entities.File;

namespace CloudDataProtection.Functions.BackupDemo.Business
{
    public class FileManagerLogic
    {
        private readonly IDataTransformer _transformer;
        
        private readonly IFileRepository _repository;

        private readonly IBlobStorageFileService _blobStorageFileService;
        private readonly IS3FileService _s3FileService;
        private readonly IGoogleCloudStorageFileService _googleCloudStorageFileService;

        private const int FilenameHashWorkFactor = 4;

        public FileManagerLogic(IBlobStorageFileService blobStorageFileService, 
            IS3FileService s3FileService,
            IGoogleCloudStorageFileService googleCloudStorageFileService, 
            IDataTransformer transformer,
            IFileRepository repository)
        {
            _blobStorageFileService = blobStorageFileService;
            _s3FileService = s3FileService;
            _googleCloudStorageFileService = googleCloudStorageFileService;
            _transformer = transformer;
            _repository = repository;
        }

        public async Task<BusinessResult<File>> Upload(IFormFile input, ICollection<FileDestination> destinations)
        {
            string fileName = GenerateFileName(input);
            
            using (Stream stream = _transformer.Encrypt(input.OpenReadStream()))
            {
                File file = new File
                {
                    ContentType = input.ContentType,
                    DisplayName = input.FileName,
                    Bytes = input.Length
                };
                
                foreach (FileDestination destination in destinations)
                {
                    IFileService service = ResolveFileService(destination);

                    FileDestinationInfo info = new FileDestinationInfo(destination);

                    try
                    {
                        MemoryStream copy = new MemoryStream();

                        await stream.CopyToAndSeekAsync(copy);

                        UploadFileResult fileResult = await service.Upload(copy, fileName);

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
            byte[] data = new byte[0];
            FileDestination? downloadedFrom = null;

            for (int i = 0; i < file.UploadedTo.Count && !downloaded; i++)
            {
                FileDestinationInfo info = file.UploadedTo[i];

                try
                {
                    Stream response = await ResolveFileService(info.Destination).GetDownloadStream(info.FileId);
                    
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
            
            FileDownloadResult result = new FileDownloadResult
            {
                Bytes = data,
                FileName = file.DisplayName,
                ContentType = file.ContentType,
                DownloadedFrom = downloadedFrom
            };
            
            return BusinessResult<FileDownloadResult>.Ok(result);
        }

        private string GenerateFileName(IFormFile input)
        {
            return Path.GetRandomFileName().Split('.')[0];
        }

        private IFileService ResolveFileService(FileDestination destination)
        {
            switch (destination)
            {
                case FileDestination.AmazonS3:
                    return _s3FileService;
                case FileDestination.AzureBlobStorage:
                    return _blobStorageFileService;
                case FileDestination.GoogleCloudStorage:
                    return _googleCloudStorageFileService;
                default:
                    throw new ArgumentException(nameof(destination));
            }
        }
    }
}