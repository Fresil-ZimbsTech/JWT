using JWTImplementation.Models;

namespace JWTImplementation.Interfaces
{
    public interface IEmployeeService
    {
        public List<Employee> GetAllEmployees();   
        public Employee GetEmployeeById(int id);
        public Employee AddEmployee(Employee employee);

        public Employee UpdateEmployee(Employee employee);
        public bool  DeleteEmployee(int id);

    }
}
