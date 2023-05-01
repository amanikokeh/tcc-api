using TCC_API.Data;
using TCC_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_API.Services
{
    internal class ExamService : IExamService
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ICollection<Exam> Index()
        {
            return context.Exams.Include(e => e.Subject).ToList();
        }


        public async Task<bool> Create(Exam exam)
        {
            try
            {
                await context.Exams.AddAsync(exam);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public async Task<bool> Update(Exam exam)
        {
            try
            {
                context.Update(exam);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(Exam exam)
        {
            context.Exams.Remove(exam);
            context.SaveChanges();
            return true;
        }

        public Exam? Show(int id)
        {
            return context.Exams.Include(e => e.ExamMarks).Include(e => e.Subject).FirstOrDefault(e => e.Id == id);
        }

        public List<ExamMark> ShowMarks(int id)
        {
            return context.Exams
                .Include(e => e.ExamMarks)
                .ThenInclude(m => m.Student)
                .First(e => e.Id == id).ExamMarks.ToList();
        }

        public List<Student> GetStudentsNotTakeExam(int id)
        {
            var studentsWithoutExam = (from s in context.Students
                                      where !context.ExamMarks.Any(em => em.StudentId == s.Id && em.ExamId == id)
                                      select s).ToList();

            return studentsWithoutExam;
        }
        public List<Student> GetStudentsTakeExam(int id)
        {
            var studentsWithExam = (from s in context.Students
                                      where context.ExamMarks.Any(em => em.StudentId == s.Id && em.ExamId == id)
                                      select s).ToList();

            return studentsWithExam;
        }

       
    }
}
