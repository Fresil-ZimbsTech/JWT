using JWTImplementation.Models;

namespace JWTImplementation.Interfaces
{
    public interface IAuthService
    {
        User AddUser(User user);
        string Login(LoginRequest loginRequest);    
    }
}
