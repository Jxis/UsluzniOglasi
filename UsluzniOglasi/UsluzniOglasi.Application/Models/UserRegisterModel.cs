using Microsoft.AspNetCore.Http;
using UsluzniOglasi.Domain.Models;

namespace UsluzniOglasi.Application.Models
{
    public class UserRegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
