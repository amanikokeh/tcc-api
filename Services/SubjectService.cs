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
    internal class SubjectService : ISubjectService
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ICollection<Subject> Index()
        {
            return context.Subjects.Include(s=>s.Department).Include(s => s.SubjectLectures).ToList();
        }

        public async Task<bool> Create(Subject s)
        {
            try
            {
                await context.Subjects.AddAsync(s);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public async Task<bool> Update(Subject s)
        {
            try
            {
                context.Update(s);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> Delete(Subject s)
        {

            context.Subjects.Remove(s);
            context.SaveChanges();
            return true;

        }

        public Subject? Show(int id)
        {
            return context.Subjects
                .Include(s => s.Department)
                .Include(s => s.SubjectLectures)
                .FirstOrDefault(s => s.Id == id);
        }


        public List<Subject> ShowByDepartment(int dept_id)
        {
            return context.Subjects
                .Include(s => s.Department)
                .Include(s => s.SubjectLectures)
                .Where(s => s.DeptId == dept_id).ToList();
        }
        public List<Subject> ShowByYear(int year)
        {
            return context.Subjects
                .Include(s => s.Department)
                .Include(s => s.SubjectLectures)
                .Where(s => s.Year == year).ToList();
        }
        public List<Subject> ShowByTerm(int term)
        {
            return context.Subjects
                .Include(s => s.Department)
                .Include(s => s.SubjectLectures)
                .Where(s => s.Term == term).ToList();
        }

        public List<SubjectLecture> ViewLectures(int id)
        {
            return context.SubjectLectures
                .Where(l => l.SubjectId == id)
                .ToList();
        }
    }
}
