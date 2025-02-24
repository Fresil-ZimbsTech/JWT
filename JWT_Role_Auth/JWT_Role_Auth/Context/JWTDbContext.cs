using JWT_Role_Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Role_Auth.Context
{
    public class JWTDbContext: DbContext
    {
        public JWTDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Employee> Employee { get; set; }

    }
}
