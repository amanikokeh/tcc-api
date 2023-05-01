using Microsoft.AspNetCore.Mvc;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IStudentService service = new StudentService();

        // GET: api/<StudentController>
        [HttpGet]
        public IActionResult Get()
        {
            var students = service.Index().Select(s => new
            {
                id = s.Id,
                first_name = s.FirstName,
                last_name = s.LastName,
                username = s.Username,
                year = s.Year,
                email = s.Email,
                phone = s.Phone,
                register_date = s.RegisterDate.ToShortDateString().ToString(),
                department = s.Department.Name,
            });
            return Ok(students);
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var s = service.Show(id);
            if (s == null)
            {
                return Ok("Couldn't find student");
            }
            return Ok(new
            {
                id = s.Id,
                first_name = s.FirstName,
                last_name = s.LastName,
                username = s.Username,
                year = s.Year,
                email = s.Email,
                phone = s.Phone,
                register_date = s.RegisterDate.ToShortDateString().ToString(),
                department = s.Department.Name,
            });
        }


        [HttpGet("GetAllSubjects/{id}")]
        public IActionResult GetAllSubjects(int id)
        {
            var student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            var s = service.ViewSubjects(id);
            return Ok(s.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                department = s.Department.Name,
                year = s.Year,
                term = s.Term,
                min_degree = s.MinDegree,
                lectures_count = s.SubjectLectures.Count,
            }));
        }
        
        [HttpGet("GetSubjectsByYear/{id}/{year}")]
        public IActionResult GetSubjectsByYear(int id,int year)
        {
            var student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            var s = service.ViewSubjectsByYear(id,year);
            return Ok(s.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                year = s.Year,
                term = s.Term,
                min_degree = s.MinDegree,
            }));
        }
        
        [HttpGet("GetSubjectsByTerm/{id}/{term}")]
        public IActionResult GetSubjectsByTerm(int id,short term)
        {
            var student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            var s = service.ViewSubjectsByTerm(id,term);
            
            return Ok(s.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                year = s.Year,
                term = s.Term,
                min_degree = s.MinDegree,
            }));
        }

        [HttpGet("GetSubjectsByDepartment/{id}/{dept}")]
        public IActionResult GetSubjectsByDepartment(int id,int dept)
        {
            var student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            var s = service.ViewSubjectsByDepartment(id,dept);
            
            return Ok(s.Select(s => new
            {
                id = s.Id,
                name = s.Name,
                year = s.Year,
                term = s.Term,
                min_degree = s.MinDegree,
            }));
        }

        [HttpGet("GetAvarage/{id}")]
        public async Task<IActionResult> GetAvarage(int id)
        {
            var student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            double s = await service.GetAvarage(id);
            
            return Ok(s);
        }

        // GET api/<StudentController>/GetMarks/5
        [HttpGet("GetMarks/{id}")]
        public IActionResult GetMarks(int id)
        {
            var marks = service.showMarks(id).Select(m => new
            {
                id = m.Id,
                subject = m.Exam.Subject.Name,
                mark = m.Mark,
            });
            return Ok(marks);
        }
        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["first_name"].ToString() == "" || fc["last_name"].ToString() == "" || 
                fc["username"].ToString() == "" || fc["email"].ToString() == "" || 
                fc["phone"].ToString() == "" || fc["year"].ToString() == "" || 
                fc["department_id"].ToString() == "")
            {
                return Ok("enter a valid data!");
            }
            Student new_student = new Student()
            {
                FirstName = fc["first_name"].ToString(),
                LastName = fc["last_name"].ToString(),
                Username = fc["username"].ToString(),
                Email = fc["email"].ToString(),
                Phone = fc["phone"].ToString(),
                Year = short.Parse(fc["year"]),
                RegisterDate = DateTime.Now,
                DepartmentId = int.Parse(fc["department_id"])
            };
            bool res = await service.Create(new_student);
            if (res)
            {
                return Ok("Created student successfuly");
            }
            return Ok("couldn't create student");
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            Student? student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student");
            }
            student.FirstName = fc["first_name"].ToString() == "" ? student.FirstName : fc["first_name"].ToString();
            student.LastName = fc["last_name"].ToString() == "" ? student.LastName : fc["last_name"].ToString();
            student.Email = fc["email"].ToString() == "" ? student.Email : fc["email"].ToString();
            student.Username = fc["username"].ToString() == "" ? student.Username : fc["username"].ToString();
            student.RegisterDate = DateTime.Now;
            student.Year = fc["year"].ToString() == "" ? student.Year : short.Parse(fc["year"].ToString());
            student.DepartmentId = fc["dept_id"].ToString() == "" ? student.DepartmentId : short.Parse(fc["dept_id"].ToString());
            student.Phone = fc["phone"].ToString() == "" ? student.Phone : fc["phone"].ToString();

            bool res = await service.Update(student);
            if (res)
            {
                return Ok("Updated Student Successfully");
            }
            return Ok("Couldn't update student");
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Student? student = service.Show(id);
            if (student == null)
            {
                return Ok("Couldn't find student!");
            }
            bool res = await service.Delete(student);
            if (res)
            {
                return Ok("Deleted student successfuly");
            }
            return Ok("couldn't delete student");
        }
    }
}
