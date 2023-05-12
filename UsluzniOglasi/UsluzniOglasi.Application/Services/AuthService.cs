using Microsoft.AspNetCore.Identity;
using UsluzniOglasi.Application.Models;
using UsluzniOglasi.Domain.Models;
using UsluzniOglasi.Infrastructure.Context;

namespace UsluzniOglasi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(UserLoginModel userLoginModel)
        {
            if (!await UserExists(userLoginModel.Email))
            {
                return false;
            }
            var user = await _userManager.FindByEmailAsync(userLoginModel.Email);
            var passwordCheck = await _userManager.CheckPasswordAsync(user, userLoginModel.Password);

            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, userLoginModel.Password, false, false);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> Register(UserRegisterModel userRegisterModel)
        {
            if (!await UserExists(userRegisterModel.Email))
            {
                var newUser = new AppUser()
                {
                    Email = userRegisterModel.Email,
                    UserName = userRegisterModel.UserName
                };

                var newUserResponse = await _userManager.CreateAsync(newUser, userRegisterModel.Password);

                if (newUserResponse.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        private async Task<bool> UserExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user != null ? true : false;
        }
    }
}
