﻿
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Data.Contexts;
using Data.Entites;
using FileProvider.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FileProvider.Services
{
    public class FileService(DataContext context, ILogger<FileService> logger, BlobServiceClient client)
    {
        private readonly DataContext _context = context;
        private readonly ILogger<FileService> _logger = logger;
        private readonly BlobServiceClient _client = client;
        private BlobContainerClient _container;

        public async Task SetBlobContainerAsync (string ContainerName)
        {
            _container = _client.GetBlobContainerClient(ContainerName);
            await _container.CreateIfNotExistsAsync ();
        }

        public string SetFileName (IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            return fileName;
        }

        public async Task<string> UploadFileAsync(IFormFile file, FileEntity fileEntity)
        {


            BlobHttpHeaders headers = new() 
            {
                ContentType = fileEntity.ContentType,
            };
            var blobClient = _container.GetBlobClient(fileEntity.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, headers);

            return blobClient.Uri.ToString();
        }

        public async Task SaveToDataBase(FileEntity fileEntity)
        {
            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync();
        }
    }
}
