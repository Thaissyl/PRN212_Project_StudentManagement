using PRN212_Project_StudentManagement.DTOs;
using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface IStudentRepository
    {
        IEnumerable<StudentDTO> GetAllStudents();
        void UpdateStudent(StudentDTO student);
        IEnumerable<string> GetAllClassNames();
        void AddStudent(StudentDTO student);
        void DeleteStudent(int studentId);
        IEnumerable<StudentDTO> GetUnassignedStudents();
        void AssignStudentToClass(int studentId, string className);
        IEnumerable<Mark> GetStudentMarks(int studentId);
        IEnumerable<StudentMarkDTO> GetStudentMarksWithSubjects(int studentId);
    }
}
