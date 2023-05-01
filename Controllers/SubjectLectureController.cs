using Microsoft.AspNetCore.Mvc;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectLectureController : ControllerBase
    {
        ISubjectLecturesService service = new SubjectLectureService();

        // GET: api/<SubjectLectureController>
        [HttpGet]
        public IActionResult Get()
        {
            var lectures = service.Index()
                .Select(l => new { 
                    id = l.Id,
                    title = l.Title,
                    content = l.Content
                });
            return Ok(lectures);
        }

        // GET api/<SubjectLectureController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var lecture = service.Show(id);
            if (lecture == null)
            {
                return Ok("Couldn't find lecture");
            }
            return Ok(new
            {
                id = lecture.Id,
                subject = lecture.Subject.Name,
                title = lecture.Title,
                content = lecture.Content
            });
        }

        // POST api/<SubjectLectureController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["subject_id"].ToString() == "" || fc["title"].ToString() == "" || fc["content"].ToString() == "")
            {
                return Ok("Enter valid data");
            }
            SubjectLecture new_lecture = new SubjectLecture()
            {
                SubjectId = int.Parse(fc["subject_id"].ToString()),
                Title = fc["title"].ToString(),
                Content = fc["content"].ToString()
            };
            bool res = await service.Create(new_lecture);
            if (res)
            {
                return Ok("created lecture successfuly");
            }
            return Ok("couldn't create lecture");
        }

        // PUT api/<SubjectLectureController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            SubjectLecture? lecture = service.Show(id);
            if (lecture == null)
            {
                return Ok("Couldn't find lecture");
            }
            lecture.SubjectId = fc["subject_id"].ToString() == "" ? lecture.SubjectId : int.Parse(fc["subject_id"].ToString());
            lecture.Title = fc["title"].ToString() == "" ? lecture.Title : fc["title"].ToString();
            lecture.Content = fc["content"].ToString() == "" ? lecture.Content : fc["content"].ToString();

            bool res = await service.Update(lecture);
            if (res)
            {
                return Ok("updated lecture lecture successfuly");
            }
            return Ok("couldn't update lecture lecture");
        }

        // DELETE api/<SubjectLectureController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            SubjectLecture? lecture = service.Show(id);
            if (lecture == null)
            {
                return Ok("Couldn't find lecture!");
            }
            bool res = await service.Delete(lecture);
            if (res)
            {
                return Ok("Deleted lecture successfuly");
            }
            return Ok("couldn't delete lecture");
        }
    }
}
