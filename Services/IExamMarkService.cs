using TCC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_API.Services
{
    internal interface IExamMarkService
    {
        public ICollection<ExamMark> Index();

        public ExamMark? Show(int id);

        public Task<bool> Create(ExamMark exam_mark);

        public Task<bool> Update(ExamMark exam_mark);

        public Task<bool> Delete(ExamMark em);
    }
}
