using JWT_Role_Auth.Models;

namespace JWT_Role_Auth.Interfaces
{
    public interface IAuthService
    {
        String Login(LoginRequest loginRequest);
        bool AssignRoleToUser(AddUserRole Add);
        User AddUser(User user);
        Role AddRole(Role role);
    }
}
