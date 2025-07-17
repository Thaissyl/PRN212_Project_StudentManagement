using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface IClassRepository
    {
        void AddClass(Class @class);
        void DeleteClass(int classId);
        List<Class> GetAllClasses();
        void UpdateClass(Class selectedClass);
    }
}
