using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212_Project_StudentManagement.Models;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface ISubjectRepository
    {
        List<Subject> GetAllSubjects();
        void DeleteSubject(int subjectId);
    }
}
