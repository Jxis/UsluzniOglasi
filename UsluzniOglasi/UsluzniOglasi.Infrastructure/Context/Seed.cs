using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UsluzniOglasi.Domain.Models;

namespace UsluzniOglasi.Infrastructure.Context
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Deliverer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Deliverer));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "AdminUserName",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "Ulica 123",
                            City = "Zrenjanin",
                            PostalCode = 23000
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Admin123!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "UserUserName",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "Ulica 456",
                            City = "Zrenjanin",
                            PostalCode = 23000
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "User123!");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
                string appDelivererEmail = "deliverer@gmail.com";

                var appDeliverer = await userManager.FindByEmailAsync(appDelivererEmail);
                if (appDeliverer == null)
                {
                    var newAppDeliverer = new AppUser()
                    {
                        UserName = "DelivererUserName",
                        Email = appDelivererEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "Ulica 789",
                            City = "Zrenjanin",
                            PostalCode = 23000
                        }
                    };
                    await userManager.CreateAsync(newAppDeliverer, "Deliverer123!");
                    await userManager.AddToRoleAsync(newAppDeliverer, UserRoles.Deliverer);
                }
            }
        }
    }
}
