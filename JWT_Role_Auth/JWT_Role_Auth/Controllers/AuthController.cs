using JWT_Role_Auth.Interfaces;
using JWT_Role_Auth.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT_Role_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth;

        public AuthController(IAuthService auth)
        {
            this.auth = auth;
        }


       
        [HttpPost("login")]
        public string Login([FromBody] LoginRequest log)
        {
            var token = auth.Login(log);
            return token;
        }

       
        [HttpPost("AssignRole")]
        public bool AssignRoleToUSer([FromBody] AddUserRole addUserRole) 
        {
            var add = auth.AssignRoleToUser(addUserRole);
            return add;
        }

       
        [HttpPost("AddUser")]
        public User Adduser([FromBody]User user)
        {
            var adduser = auth.AddUser(user);
            return adduser;
        }

    
        [HttpPost("addrole")]
        public Role AddRole([FromBody]Role role)
        {
            var add = auth.AddRole(role);
            return add;
        }

     
      
    }
}
