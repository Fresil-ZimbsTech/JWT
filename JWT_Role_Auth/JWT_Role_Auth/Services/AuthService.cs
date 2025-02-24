using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_Role_Auth.Context;
using JWT_Role_Auth.Interfaces;
using JWT_Role_Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Role_Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTDbContext context;
        private readonly IConfiguration config;

        public AuthService(JWTDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }




        public Role AddRole(Role role)
        {
            var addRole = context.Role.Add(role);
            context.SaveChanges();
            return addRole.Entity;
        }

        public User AddUser(User user)
        {
            var addUser = context.User.Add(user);
            context.SaveChanges();
            return addUser.Entity;
        }

        public bool AssignRoleToUser(AddUserRole Add)
        {
            try
            {
                var addRole = new List<UserRole>();
                var user = context.User.FirstOrDefault(x => x.Id == Add.UserId);
                if (user == null)
                {
                    throw new Exception("USer is not valid");
                }
                foreach (int role in Add.RoleIds)
                {
                    var userRole = new UserRole();
                    userRole.RoleId = role;
                    userRole.UserId = user.Id;
                    addRole.Add(userRole);
                }
                context.UserRole.AddRange(addRole);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Login(LoginRequest loginRequest)
        {
            if(loginRequest.UserName != null && loginRequest.Password != null)
            {
                var user = context.User.FirstOrDefault(x=>x.UserName == loginRequest.UserName && x.Password==loginRequest.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
                        new Claim("Id",user.Id.ToString()),
                        new Claim("UserName",user.Name) // check it after run replace it with username
                    };
                    var userRole = context.UserRole.Where(x=>x.UserId == user.Id).ToList();
                    var roleId = userRole.Select(x => x.RoleId).ToList();
                    var roles = context.Role.Where(x=>roleId.Contains(x.Id)).ToList();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                    
                    
                    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        config["Jwt:Issuer"],
                        config["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("User is not valid");
                }
            }
            else
            {
                throw new Exception("Credential are not valid");
            }
        }
    }
}
