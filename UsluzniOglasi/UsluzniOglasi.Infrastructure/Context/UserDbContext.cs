using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsluzniOglasi.Domain.Models;

namespace UsluzniOglasi.Infrastructure.Context
{
    public class UserDbContext : IdentityDbContext<AppUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
    }
}
