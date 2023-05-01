using TCC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_API.Services
{
    internal interface IExamService
    {
        public ICollection<Exam> Index();

        public Task<bool> Create(Exam exam);

        public Task<bool> Update(Exam exam);

        public Task<bool> Delete(Exam exam);

        public Exam? Show(int id);
        public List<ExamMark> ShowMarks(int id);

        public List<Student> GetStudentsNotTakeExam(int id);
        public List<Student> GetStudentsTakeExam(int id);
    }
}
