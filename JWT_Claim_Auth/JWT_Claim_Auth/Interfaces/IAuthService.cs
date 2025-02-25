using JWT_Claim_Auth.Models;

namespace JWT_Claim_Auth.Interfaces
{
    public interface IAuthService
    {

        
            User AddUser(User user);
            string Login(LoginRequest loginRequest);
        
    }
}
