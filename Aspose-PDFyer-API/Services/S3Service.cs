using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AsposeTriage.Services.Interfaces;
using AsposeTriage.Common;
using System.Net;

namespace AsposeTriage.Services
{
    public class S3Service: IS3Service
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _amazonS3Client;

        public S3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            var credentials = new BasicAWSCredentials(_configuration["S3:AK"], _configuration["S3:SAK"]);
            _amazonS3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
        }

        public async Task<Tuple<Stream, string>> GetFileFromS3(string directory, string key)
        {
            var request = new GetObjectRequest()
            {
                BucketName = _configuration["S3:BucketName"],
                Key = $"{directory}/{key}",
            };
            var response = await _amazonS3Client.GetObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return new Tuple<Stream, string>(response.ResponseStream, response.Headers.ContentType);
            }
            return new Tuple<Stream, string>(Stream.Null, string.Empty); 
        }

        public async Task<bool> PutFileInS3(IFormFile file, string directory)
        {
            if(file == null)
            {
                ArgumentNullException.ThrowIfNull(Messages.FileRequired);
                return false;
            }
            return await this.LoadStreamInS3(file.OpenReadStream(), directory, file.FileName, Path.GetExtension(file.FileName), file.ContentType);
        }

        public async Task<bool> LoadStreamInS3(Stream stream, string directory, string key, string extension, string contentType)
        {
            if (stream == null)
            {
                ArgumentNullException.ThrowIfNull(Messages.FileRequired);
                return false;
            }
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = _configuration["S3:BucketName"],
                Key = $"{directory}/{key}",
                InputStream = stream,
                ContentType = contentType,
                Metadata =
                  {
                      ["x-amz-meta-original-file-name"] = key,
                      ["x-amz-meta-original-file-extension"] = extension,
                  }
            };
            var response = await _amazonS3Client.PutObjectAsync(putObjectRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteFileInS3(string directory, string key)
        {
            var request = new DeleteObjectRequest()
            {
                BucketName = _configuration["S3:BucketName"],
                Key = $"{directory}/{key}",
            };

            var response = await _amazonS3Client.DeleteObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }
    }
}
