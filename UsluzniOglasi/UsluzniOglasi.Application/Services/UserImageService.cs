using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
        public async Task<DeleteObjectResponse?> DeleteImageAsync(string id)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = GetBucketName(),
                    Key = $"profile_images/{id}"
                };
                return await _s3.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception ex) when
            (ex.Message is "The specified key does not exist.")
            {
                return null;
            }
        }
        public async Task<GetObjectResponse?> GetImageAsync(string id)
        {
            try
            {
                var getObjectRequest = new GetObjectRequest
                {
                    BucketName = GetBucketName(),
                    Key = $"profile_images/{id}"
                };
                return await _s3.GetObjectAsync(getObjectRequest);
            }
            catch (AmazonS3Exception ex) when
            (ex.Message is "The specified key does not exist.")
            {
                return null;
            }
        }
        public async Task<PutObjectResponse> UploadImageAsync(string id, IFormFile file)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = GetBucketName(),
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
        private string? GetBucketName()
        {
            return _configuration.GetSection("AWS:BucketName").Value;
        }
    }
}
