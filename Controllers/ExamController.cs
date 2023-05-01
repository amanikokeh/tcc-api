using Microsoft.AspNetCore.Mvc;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        IExamService service = new ExamService();
        ISubjectService subjectService = new SubjectService();

        // GET: api/<ExamController>
        [HttpGet]
        public IActionResult Get()
        {
            var exams = service.Index()
                .Select(s => new {
                    id = s.Id,
                    subject = s.Subject.Name,
                    date = s.Date.ToShortDateString().ToString(),
                    term = s.Term
                    });
            return Ok(exams);
        }

        // GET api/<ExamController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var exam = service.Show(id);
            if (exam == null)
            {
                return Ok("Couldn't find exam");
            }
            return Ok(new { 
                id = exam.Id, 
                subject = exam.Subject.Name,
                date = exam.Date.ToShortDateString().ToString(),
                term = exam.Term
            });
        }
        
        // GET api/<ExamController>/GetMarks/5
        [HttpGet("GetMarks/{id}")]
        public IActionResult GetMarks(int id)
        {
            var marks = service.ShowMarks(id).Select(m => new
            {
                id = m.Id,
                student = m.Student.FirstName+" "+m.Student.LastName,
                mark = m.Mark,
            });
            return Ok(marks);
        }

        [HttpGet("GetStudentsNotTakeExam/{id}")]
        public IActionResult GetStudentsNotTakeExam(int id)
        {
            var students = service.GetStudentsNotTakeExam(id).Select(s => new
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
            return Ok(students);
        }
        
        [HttpGet("GetStudentsTakeExam/{id}")]
        public IActionResult GetStudentsTakeExam(int id)
        {
            var students = service.GetStudentsTakeExam(id).Select(s => new
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
            return Ok(students);
        }

        // POST api/<ExamController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["subject_id"].ToString() == "" || subjectService.Show(int.Parse(fc["subject_id"])) == null){
                return Ok("Enter an existing subject");
            }
            if (fc["term"].ToString() == "" || int.Parse(fc["term"]) <=0 || int.Parse(fc["term"]) > 3)
            {
                return Ok("Enter a valid term");
            }
            if (fc["date"].ToString() == "")
            {
                return Ok("Enter a date");
            }
            Exam new_exam = new Exam()
            {
                SubjectId = int.Parse(fc["subject_id"]),
                Term = short.Parse(fc["term"]),
                Date = DateTime.Parse(fc["date"].ToString())
            };
            bool res = await service.Create(new_exam);
            if (res)
            {
                return Ok("Created Exam Successfully");
            }
            return Ok("Couldn't create exam");
        }

        // PUT api/<ExamController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            Exam? exam = service.Show(id);
            if (exam == null)
            {
                return Ok("Couldn't find exam");
            }
            exam.SubjectId = fc["subject_id"].ToString() == "" ? exam.SubjectId : int.Parse(fc["subject_id"].ToString());
            exam.Term = fc["term"].ToString() == "" ? exam.Term : short.Parse(fc["term"].ToString());
            exam.Date = fc["date"].ToString() == "" ? exam.Date : DateTime.Parse(fc["date"].ToString());

            bool res = await service.Update(exam);
            if (res)
            {
                return Ok("Updated Exam Successfully");
            }
            return Ok("Couldn't update exam");
        }

        // DELETE api/<ExamController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Exam? exam = service.Show(id);
            if (exam == null)
            {
                return Ok("Couldn't find exam!");
            }
            bool res = await service.Delete(exam);
            if (res)
            {
                return Ok("Deleted exam successfuly");
            }
            return Ok("couldn't delete exam");
        }
    }
}
