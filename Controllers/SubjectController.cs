using Microsoft.AspNetCore.Mvc;
using TCC_API.Models;
using TCC_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TCC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        ISubjectService service = new SubjectService();

        // GET: api/<SubjectController>
        [HttpGet]
        public IActionResult Get()
        {
            var subjects = service.Index()
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    department =  s.Department.Name,
                    year = s.Year,
                    term = s.Term,
                    min_degree = s.MinDegree,
                    lectures_count = s.SubjectLectures.Count,
                });
            return Ok(subjects);
        }
        
        [HttpGet("GetByDepartment/{dept}")]
        public IActionResult GetByDepartment(int dept)
        {
            var subjects = service.ShowByDepartment(dept)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    department =  s.Department.Name,
                    year = s.Year,
                    term = s.Term,
                    min_degree = s.MinDegree,
                    lectures_count = s.SubjectLectures.Count,
                });
            return Ok(subjects);
        }

        [HttpGet("GetByYear/{year}")]
        public IActionResult GetByYear(int year)
        {
            var subjects = service.ShowByYear(year)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    department =  s.Department.Name,
                    year = s.Year,
                    term = s.Term,
                    min_degree = s.MinDegree,
                    lectures_count = s.SubjectLectures.Count,
                });
            return Ok(subjects);
        }
        

        [HttpGet("GetByTerm/{term}")]
        public IActionResult GetByTerm(int term)
        {
            var subjects = service.ShowByTerm(term)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    department =  s.Department.Name,
                    year = s.Year,
                    term = s.Term,
                    min_degree = s.MinDegree,
                    lectures_count = s.SubjectLectures.Count,
                });
            return Ok(subjects);
        }

        // GET api/<SubjectController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var s = service.Show(id);
            if (s == null)
            {
                return Ok("Couldn't find subject");
            }
            return Ok(new
            {
                id = s.Id,
                name = s.Name,
                department = s.Department.Name,
                year = s.Year,
                term = s.Term,
                min_degree = s.MinDegree,
                lectures_count = s.SubjectLectures.Count,
            });
        }

        // GET api/<SubjectController>/5
        [HttpGet("GetLectures/{id}")]
        public IActionResult GetLectures(int id)
        {
            var lectures = service.ViewLectures(id)
                .Select(l => new
                {
                    id = l.Id,
                    title = l.Title,
                    content = l.Content,
                });
            return Ok(lectures);
        }

        // POST api/<SubjectController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormCollection fc)
        {
            if (fc["name"].ToString() == "" 
                || fc["dept_id"].ToString() == ""
                || fc["year"].ToString() == ""
                || fc["term"].ToString() == ""
                || fc["min_degree"].ToString() == ""
                )
            {
                return Ok("Enter a valid data");
            }
            Subject new_subject = new Subject()
            {
                Name = fc["name"].ToString(),
                DeptId = int.Parse(fc["dept_id"].ToString()),
                Year = short.Parse(fc["year"].ToString()),
                Term = short.Parse(fc["term"].ToString()),
                MinDegree = short.Parse(fc["min_degree"].ToString()),
            };
            bool res = await service.Create(new_subject);
            if (res)
            {
                return Ok("Created subject successfuly");
            }
            return Ok("couldn't create subject");
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] IFormCollection fc)
        {
            Subject? subject = service.Show(id);
            if (subject == null)
            {
                return Ok("Couldn't find subject");
            }
            subject.DeptId = fc["dept_id"].ToString() == "" ? subject.DeptId : int.Parse(fc["dept_id"].ToString());
            subject.Name = fc["name"].ToString() == "" ? subject.Name : fc["name"].ToString();
            subject.Year = fc["year"].ToString() == "" ? subject.Year : short.Parse(fc["year"].ToString());
            subject.Term = fc["term"].ToString() == "" ? subject.Term : short.Parse(fc["term"].ToString());
            subject.MinDegree = fc["term"].ToString() == "" ? subject.Term : short.Parse(fc["term"].ToString());

            bool res = await service.Update(subject);
            if (res)
            {
                return Ok("updated subject subject successfuly");
            }
            return Ok("couldn't update subject subject");
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Subject? subject = service.Show(id);
            if (subject == null)
            {
                return Ok("Couldn't find subject!");
            }
            bool res = await service.Delete(subject);
            if (res)
            {
                return Ok("Deleted subject successfuly");
            }
            return Ok("couldn't delete subject");
        }
    }
}
