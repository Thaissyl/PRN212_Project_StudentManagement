using System;

namespace PRN212_Project_StudentManagement.DTOs
{
    public class StudentClassDTO
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int AcademicFromYear { get; set; }
        public int AcademicToYear { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public string AcademicYearRange => $"{AcademicFromYear} - {AcademicToYear}";
    }
} 