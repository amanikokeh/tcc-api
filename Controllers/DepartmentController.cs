using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Xml.Linq;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        IDepartmentService service = new DepartmentService();

        // GET: api/<DepartmentController>
        [HttpGet]
        public IActionResult Get()
        {
            var departments = service.Index().Select(s => new { s.Id, s.Name });
            return Ok(departments);
        }

        // GET api/<DepartmentController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var department = service.Show(id);
            if (department == null)
            {
                return Ok("Couldn't find department");
            }
            return Ok(new { id = department.Id, name = department.Name });
        }
        // GET api/<DepartmentController>/5
        [HttpGet("GetStudents/{id}")]
        public IActionResult GetStudents(int id)
        {
            var students = service.ShowStudents(id).Select(s => new {
                id = s.Id,
                first_name = s.FirstName,
                last_name = s.LastName,
                year = s.Year,
                username = s.Username,
                email = s.Email,
                phone = s.Phone,
                register_date = s.RegisterDate.ToShortDateString().ToString(),
                
            });
            return Ok(students);
        }
        
        // GET api/<DepartmentController>/5
        [HttpGet("GetStudentsByYear/{id}/{year}")]
        public IActionResult GetStudentsByYear(int id,int year)
        {   
            var students_by_year = service.ViewStudentsByYear(id, year)
                .Select(s => new
                {
                    id = s.Id,
                    first_name = s.FirstName,
                    last_name = s.LastName,
                    year = s.Year,
                    username = s.Username,
                    email = s.Email,
                    phone = s.Phone,
                    register_date = s.RegisterDate.ToShortDateString().ToString(),
                });
            return Ok(students_by_year);
        }

        // GET api/<DepartmentController>/5
        [HttpGet("GetSubjects/{id}")]
        public IActionResult GetSubjects(int id)
        {
            var subjects = service.ShowSubjects(id).Select(s => new {
                id = s.Id,
                name = s.Name,
                min_degree = s.MinDegree,
                year = s.Year,
                term = s.Term,
                lectures_count = s.SubjectLectures.Count                
            });
            return Ok(subjects);
        }

        // POST api/<DepartmentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["name"] == "")
            {
                return Ok("Enter Name Please!");
            }
            Department new_dept = new Department() {
                Name = fc["name"].ToString()
            };
            bool res =await service.Create(new_dept);
            if (res)
            {
                return Ok("Created department successfuly");
            }
            return Ok("couldn't create department");
        }

        // PUT api/<DepartmentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            Department? department = service.Show(id);
            if (department == null)
            {
                return Ok("Couldn't find department!");
            }
            department.Name = fc["name"].ToString() == "" ? department.Name : fc["name"].ToString();

            bool res =await service.Update(department);
            if (res)
            {
                return Ok("Updated department successfuly");
            }
            return Ok("couldn't update department");
        }

        // DELETE api/<DepartmentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Department? department = service.Show(id);
            if (department == null)
            {
                return Ok("Couldn't find department!");
            }
            bool res = await service.Delete(department);
            if (res)
            {
                return Ok("Deleted department successfuly");
            }
            return Ok("couldn't delete department");

        }
    }
}
