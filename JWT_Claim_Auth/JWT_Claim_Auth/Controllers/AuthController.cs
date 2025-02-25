using JWT_Claim_Auth.Interfaces;
using JWT_Claim_Auth.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT_Claim_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService service;

        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        // GET: api/<AuthController>
        [HttpPost("addUser")]
        public User AddUser([FromBody] User user)
        {
            var adduser = service.AddUser(user);
            return adduser;
        }

        [HttpPost]
        public string Login([FromBody] LoginRequest login)
        {
            var user = service.Login(login);
            return user;
        }


    }
}
