using Microsoft.AspNetCore.Identity;

namespace JWT_Identity_Policy.Context
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; } // Added Role Property
    }
}
