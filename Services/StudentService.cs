using TCC_API.Data;
using TCC_API.Models;
using Microsoft.EntityFrameworkCore;

namespace TCC_API.Services
{
    internal class StudentService : IStudentService
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ICollection<Student> Index()
        {
            return context.Students.Include(s => s.Department).ToList();
        }

        public async Task<bool> Create(Student student)
        {
            try
            {
                await context.Students.AddAsync(student);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public async Task<bool> Update(Student student)
        {
            try
            {
                context.Update(student);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> Delete(Student s)
        {
            context.Students.Remove(s);
            context.SaveChanges();
            return true;
        }

        public ICollection<ExamMark> showMarks(int id)
        {
            Student? s = context.Students.Include(s => s.ExamMarks).ThenInclude(m => m.Exam).ThenInclude(e => e.Subject).First(s => s.Id == id);
            if (s == null)
            {
                return null;
            }
            return s.ExamMarks.ToList();
        }

        public async Task<double> GetAvarage(int id)
        {
            Student? student = context.Students.Include(s => s.ExamMarks).First(s => s.Id == id);
            if (student == null)
            {
                return -1;
            }
            double sum = 0;
            var exams = student.ExamMarks.ToList();
            foreach (var item in exams)
            {
                sum += item.Mark;
            }
            return sum / exams.Count;
        }

        public List<Subject> ViewSubjectsByDepartment(int id, int dept_id)
        {
            var subjects = (from student in context.Students
                            join department in context.Departments
                            on student.DepartmentId equals department.Id
                            join subject in context.Subjects
                            on department.Id equals subject.DeptId
                            where student.Id == id && department.Id == dept_id
                            select subject).ToList();
            return subjects;
        }

        public Student? Show(int id)
        {
            return context.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
        }

        public List<Subject> ViewSubjects(int id)
        {
            Student student = context.Students.Include(s => s.Department).ThenInclude(d => d.Subjects).ThenInclude(d => d.SubjectLectures).FirstOrDefault(s => s.Id == id);
            return student.Department.Subjects.ToList();
        }

        public List<Subject> ViewSubjectsByTerm(int id, short term)
        {
            var studentSubjects = (from student in context.Students
                                   join subject in context.Subjects on student.DepartmentId equals subject.DeptId
                                   where student.Id == id && subject.Term == term
                                   select subject).ToList();
            return studentSubjects;
        }
        public List<Subject> ViewSubjectsByYear(int id, int year)
        {
            var studentSubjects = (from student in context.Students
                                   join subject in context.Subjects on student.DepartmentId equals subject.DeptId
                                   where student.Id == id && subject.Year == year
                                   select subject).ToList();
            return studentSubjects;
        }
    }
}
