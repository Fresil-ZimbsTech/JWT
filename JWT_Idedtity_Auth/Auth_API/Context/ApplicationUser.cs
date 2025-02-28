using Microsoft.AspNetCore.Identity;

namespace Auth_API.Context
{
    public class ApplicationUser :IdentityUser
    {
        public string Name { get; set; }
    }
}
