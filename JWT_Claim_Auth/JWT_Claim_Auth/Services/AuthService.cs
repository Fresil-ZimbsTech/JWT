using JWT_Claim_Auth.Context;
using JWT_Claim_Auth.Interfaces;
using JWT_Claim_Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Claim_Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTDbcontext context;
        private readonly IConfiguration config;

        public AuthService(JWTDbcontext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }



        public User AddUser(User user)
        {
            var adduser = context.User.Add(user);
            context.SaveChanges();
            return adduser.Entity;

        }

        public string Login(LoginRequest loginRequest)
        {
            if (loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = context.User.SingleOrDefault(x => x.Email == loginRequest.Username && x.Password == loginRequest.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,config["Jwt:Subject"]),
                        new Claim("Id",user.Id.ToString()),
                        new Claim ("Username",user.Name),
                        new Claim ("Email",user.Email),
                        new Claim("Role",user.Role),     //Role Claim
                        new Claim("Permission",user.Permission)   // Permission Claim

                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        config["Jwt:Issuer"],
                        config["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signIn
                        );
                    var Jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
                    return Jwttoken;
                }
                else
                {
                    throw new Exception("user is not valid...");

                }
            }
            else
            {
                throw new Exception("username and password is required ...");
            }
        }
    }
}
