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
    internal class ExamMarkService : IExamMarkService
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ICollection<ExamMark> Index()
        {
            return context.ExamMarks.Include(e=>e.Student).Include(e=>e.Exam).ThenInclude(e=>e.Subject).ToList();
        }

        public ExamMark? Show(int id)
        {
            return context.ExamMarks.FirstOrDefault(m =>  m.Id == id);
        }


        public async Task<bool> Create(ExamMark em)
        {
            try
            {
                await context.ExamMarks.AddAsync(em);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public async Task<bool> Update(ExamMark em)
        {
            try
            {
                context.Update(em);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(ExamMark em)
        {
            context.ExamMarks.Remove(em);
            context.SaveChanges();
            return true;
        }
    }
}
