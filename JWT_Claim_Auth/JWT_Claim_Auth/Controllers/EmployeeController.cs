using JWT_Claim_Auth.Interfaces;
using JWT_Claim_Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWT_Claim_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService service;

        public EmployeeController(IEmployeeService service)
        {
            this.service = service;
        }


        // GET: api/<EmployeeController>
        [HttpGet]
        [Authorize(Policy = "AdminOrHr")] // only hr can access
        public List<Employee> Get()
        {
            var emp = service.GetAllEmployees();
            return emp;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ViewOrEditOrUser")]   
        public Employee Get(int id)
        {
            var emp = service.GetEmployeeById(id);
            return emp;
        }

        // POST api/<EmployeeController>
        [HttpPost]
        [Authorize(Policy = "UpdateOrAdminOrDelete")]   // only admin can access
        public Employee Post([FromBody] Employee employee)
        {
            var emp = service.AddEmployee(employee);
            return emp;
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "UpdateOrDeleteOrEdit")]
        public Employee Put(int id, [FromBody] Employee employee)
        {
            var EMP = service.UpdateEmployee(employee);
            return EMP;
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UpdateOrAdminOrDelete")]
        public bool Delete(int id)
        {
            var emp = service.DeleteEmployee(id);
            return emp;
        } 
    }
}
