using JWT_Claim_Auth.Models;

namespace JWT_Claim_Auth.Interfaces
{
    public interface IEmployeeService
    {

        public List<Employee> GetAllEmployees();
        public Employee GetEmployeeById(int id);
        public Employee AddEmployee(Employee employee);

        public Employee UpdateEmployee(Employee employee);
        public bool DeleteEmployee(int id);

    }
}
