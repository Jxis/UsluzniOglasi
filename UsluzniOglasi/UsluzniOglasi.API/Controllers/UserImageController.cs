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
        [HttpPost("user/{userId}/image")]
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
    }
}
