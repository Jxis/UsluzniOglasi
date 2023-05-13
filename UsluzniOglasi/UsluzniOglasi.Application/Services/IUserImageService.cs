using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace UsluzniOglasi.Application.Services
{
    public interface IUserImageService
    {
        Task<PutObjectResponse> UploadImageAsync(string id, IFormFile file);
    }
}