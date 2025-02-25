using JWT_Claim_Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Claim_Auth.Context
{
    public class JWTDbcontext : DbContext
    {
        public JWTDbcontext(DbContextOptions<JWTDbcontext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Employee> Employee { get; set; }
    }
}

