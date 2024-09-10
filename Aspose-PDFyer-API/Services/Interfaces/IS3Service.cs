﻿namespace AsposeTriage.Services.Interfaces
{
    public interface IS3Service
    {
        public Task<Tuple<Stream, string>> GetFileFromS3(string directory, string key);
        public Task<bool> LoadStreamInS3(Stream stream, string key, string extension, string contentType);
        public Task<bool> PutFileInS3(IFormFile file, string key, string contentType);
        public Task<bool> DeleteFileInS3(string directory, string key);
    }
}
