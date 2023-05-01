using TCC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_API.Services
{
    internal interface ISubjectLecturesService
    {
        public ICollection<SubjectLecture> Index();

        public Task<bool> Create(SubjectLecture subject_lecture);

        public Task<bool> Update(SubjectLecture subject_lecture);

        public Task<bool> Delete(SubjectLecture sl);
        public SubjectLecture? Show(int id);
    }
}
