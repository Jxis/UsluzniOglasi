using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UsluzniOglasi.Application.Services;

namespace UsluzniOglasi.API.Controllers
{
    [ApiController]
    public class UserImageController : ControllerBase
    {
        private readonly IUserImageService _userImageService;
        public UserImageController(IUserImageService userImageService)
        {
            _userImageService= userImageService;
        }
        [HttpPost("users/{userId}/image")]
        public async Task<IActionResult> Upload([FromRoute] string userId,
            [FromForm(Name = "Data")] IFormFile file)
        {
            var response = await _userImageService.UploadImageAsync(userId, file);

            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                return Ok("Uploaded successfully!");
            }
            return BadRequest(response);
        }
        [HttpGet("users/{userId}/image")]
        public async Task<IActionResult> Get([FromRoute] string userId)
        {
            var response = await _userImageService.GetImageAsync(userId);
            if(response is null)
            {
                return NotFound();
            }
            return File(response.ResponseStream, response.Headers.ContentType);
        }
        [HttpDelete("users/{userId}/image")]
        public async Task<IActionResult> Delete([FromRoute] string userId)
        {
            var response = await _userImageService.DeleteImageAsync(userId);
            return response.HttpStatusCode switch
            {
                HttpStatusCode.NoContent => Ok(),
                HttpStatusCode.NotFound => NotFound(),
                _ => BadRequest()
            };
        }
    }
}
