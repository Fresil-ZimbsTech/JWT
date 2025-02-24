using JWT_Role_Auth.Models;

namespace JWT_Role_Auth.Interfaces
{
    public interface IEmployeeService
    {
        public List<Employee> GetEmployeeDetails();
        public Employee AddEmployee(Employee employee);
    }
}
