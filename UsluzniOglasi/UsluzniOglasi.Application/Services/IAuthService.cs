using UsluzniOglasi.Application.Models;

namespace UsluzniOglasi.Application.Services
{
    public interface IAuthService
    {
        Task<bool> Login(UserLoginModel userLoginModel);
        Task<bool> Logout();
        Task<bool> Register(UserRegisterModel userRegisterModel);
    }
}