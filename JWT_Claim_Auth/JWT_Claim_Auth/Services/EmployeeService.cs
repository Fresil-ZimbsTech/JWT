using JWT_Claim_Auth.Context;
using JWT_Claim_Auth.Interfaces;
using JWT_Claim_Auth.Models;

namespace JWT_Claim_Auth.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly JWTDbcontext context;

        public EmployeeService(JWTDbcontext context)
        {
            this.context = context;
        }




        public Employee AddEmployee(Employee employee)
        {
            var emp = context.Employee.Add(employee);
            context.SaveChanges();
            return emp.Entity;
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                var emp = context.Employee.FirstOrDefault(x => x.Id == id);
                if (emp != null)
                {
                    context.Employee.Remove(emp);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Employee> GetAllEmployees()
        {
            var emp = context.Employee.ToList();
            return emp;
        }

        public Employee GetEmployeeById(int id)
        {
            var emp = context.Employee.FirstOrDefault(x => x.Id == id);
            return emp;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            var emp = context.Employee.Update(employee);
            context.SaveChanges();
            return emp.Entity;
        }
    }
}
