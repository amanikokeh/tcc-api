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
    internal class DepartmentService : IDepartmentService
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ICollection<Department> Index()
        {
            return context.Departments.Include(d => d.Students).Include(d=>d.Subjects).ToList();
        }

        public async Task<bool> Create(Department student)
        {
            try
            {
                await context.Departments.AddAsync(student);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> Update(Department dept)
        {
            try
            {
                context.Update(dept);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }


        public async Task<bool> Delete(Department dept)
        {         
            context.Departments.Remove(dept);
            context.SaveChanges();
            return true;

        }

        public Department? Show(int id)
        {
            return context.Departments.Include(d => d.Students).Include(d => d.Subjects).FirstOrDefault(d => d.Id == id);
        }

        public ICollection<Student> ShowStudents(int id)
        {
            Department d = context.Departments.Include(d => d.Students).First(d => d.Id == id);
            return d.Students;
        }

        public ICollection<Subject> ShowSubjects(int id)
        {
            Department d = context.Departments.Include(d => d.Subjects).ThenInclude(s => s.SubjectLectures).First(d => d.Id == id);
            return d.Subjects;
        }

        public List<Student> ViewStudentsByYear(int id, int year)
        {
            return context.Students.Include(s => s.Department).Where(s => s.DepartmentId == id && s.Year == year).ToList();
        }
    }
}
