using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsluzniOglasi.Application.Services
{
    public class UserImageService : IUserImageService
    {
        private readonly IAmazonS3 _s3;
        private readonly IConfiguration _configuration;

        public UserImageService(IAmazonS3 s3, IConfiguration configuration)
        {
            _s3 = s3;
            _configuration = configuration;
        }
        public async Task<PutObjectResponse> UploadImageAsync(string id, IFormFile file)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _configuration.GetSection("AWS:BucketName").Value,
                Key = $"profile_images/{id}",
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
                Metadata =
                {
                    ["x-amz-meta-originalname"] = file.FileName,
                    ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
                }
            };
            return await _s3.PutObjectAsync(putObjectRequest);
        }
    }
}
