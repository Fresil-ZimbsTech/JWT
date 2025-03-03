using JWT_Identity_Policy.Context;
using JWT_Identity_Policy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_Identity_Policy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public StudentsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        [HttpGet]
        //[Authorize(Policy = "view")] // Apply Claim-Based Authorization ,teacher can view all students
        //[Authorize(Policy = "TeacherAccess")] // Teachers can view students
        [Authorize(Policy = "TeacherAgeBasedAccess")]
        [Authorize(Policy = "CanViewStudentsPolicy")]
       

        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var data = await context.Students.ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        //[Authorize(Policy = "manage")] // Only users with 'CanManageStudents' claim can add ,admin can view all
        [Authorize(Policy = "AdminOrTeacherAccess")]  
       
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }


        [HttpPost]
        //[Authorize(Policy = "AdminOrTeacherAccess")] // Admins (18+) can create a  student
        [Authorize(Policy = "TeacherAgeBasedAccess")]
        [Authorize(Policy = "CanManageStudentsPolicy")]

        public async Task<ActionResult<Student>> CreateStudent(Student std)
        {
            await context.Students.AddAsync(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "AdminAccess")] // Admins (18+) can update a specific student
        public async Task<ActionResult<Student>> UpdateStudent(int id, Student std)
        {
            if (id != std.Id)
            {
                return BadRequest();
            }
            context.Entry(std).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(std);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminAccess")] // Admins (18+) can delete a specific student
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var std = await context.Students.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.Students.Remove(std);
            await context.SaveChangesAsync();
            return Ok();
        }


        //[HttpGet("senior")]
        //[Authorize(Policy = "SeniorCitizenAccess")] // Only users <60 can access
        //public IActionResult SeniorCitizenFeature()
        //{
        //    return Ok("Feature available for users under 60.");
        //}
    }
}
