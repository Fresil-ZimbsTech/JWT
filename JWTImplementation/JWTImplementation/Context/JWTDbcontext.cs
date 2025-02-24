using JWTImplementation.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTImplementation.Context
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
