using JWT_Role_Auth.Context;
using JWT_Role_Auth.Interfaces;
using JWT_Role_Auth.Models;

namespace JWT_Role_Auth.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly JWTDbContext context;

        public EmployeeService(JWTDbContext context)
        {
            this.context = context;
        }



        public Employee AddEmployee(Employee employee)
        {
            var emp = context.Employee.Add(employee);
            context.SaveChanges();
            return emp.Entity;
        }
        public List<Employee> GetEmployeeDetails()
        {
            var emp = context.Employee.ToList();
            return emp;
        }

    }
}
