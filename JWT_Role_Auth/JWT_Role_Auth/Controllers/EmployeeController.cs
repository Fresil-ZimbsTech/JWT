using JWT_Role_Auth.Interfaces;
using JWT_Role_Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT_Role_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService service;

        public EmployeeController(IEmployeeService service )
        {
            this.service = service;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public List<Employee> GetEmployeeDetails()
        {
           var emp = service .GetEmployeeDetails();
            return emp;
        }


        // POST api/<EmployeeController>
        [HttpPost]
        public Employee AddEmployee([FromBody]Employee  emp)
        {
            var user = service.AddEmployee(emp);
            return user;
        }

        

        
    }
}
