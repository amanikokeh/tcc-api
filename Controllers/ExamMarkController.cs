using Microsoft.AspNetCore.Mvc;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamMarkController : ControllerBase
    {
        IExamMarkService service = new ExamMarkService();

        // GET: api/<ExamMarkController>
        [HttpGet]
        public IActionResult Get()
        {
            var marks = service.Index().Select( m => new { 
            id = m.Id,
            subject = m.Exam.Subject.Name,
            student = m.Student.FirstName+ " " + m.Student.LastName,
            mark = m.Mark,
            });
            return Ok(marks);
        }

        // POST api/<ExamMarkController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["exam_id"].ToString() == "" 
                || fc["student_id"].ToString() == "" 
                || fc["mark"].ToString() == "")
            {
                return Ok("enter a valid data");
            }
            ExamMark new_mark = new ExamMark()
            {
                ExamId = int.Parse(fc["exam_id"].ToString()),
                StudentId = int.Parse(fc["student_id"].ToString()),
                Mark = int.Parse(fc["mark"].ToString())
            };
            bool res = await service.Create(new_mark);
            if (res)
            {
                return Ok("created mark mark successfuly");
            }
            return Ok("couldn't create mark mark");
        }

        // PUT api/<ExamMarkController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            ExamMark? mark = service.Show(id);
            if (mark == null)
            {
                return Ok("Couldn't find mark");
            }
            mark.StudentId = fc["student_id"].ToString() == "" ? mark.StudentId : int.Parse(fc["student_id"].ToString());
            mark.ExamId = fc["exam_id"].ToString() == "" ? mark.ExamId : int.Parse(fc["exam_id"].ToString());
            mark.Mark = fc["mark"].ToString() == "" ? mark.Mark : int.Parse(fc["mark"].ToString());
            
            bool res = await service.Update(mark);
            if (res)
            {
                return Ok("updated mark mark successfuly");
            }
            return Ok("couldn't update mark mark");
        }

        // DELETE api/<ExamMarkController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ExamMark? mark = service.Show(id);
            if (mark == null)
            {
                return Ok("Couldn't find mark!");
            }
            bool res = await service.Delete(mark);
            if (res)
            {
                return Ok("Deleted mark successfuly");
            }
            return Ok("couldn't delete mark");
        }
    }
}
