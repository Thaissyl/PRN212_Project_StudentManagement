using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface ITeacherRepository
    {
        List<Teacher> GetAllTeachers();
        void AddTeacher(Teacher teacher);
        void DeleteTeacher(int teacherId);
    }
}
