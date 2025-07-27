using System;

namespace PRN212_Project_StudentManagement.DTOs
{
    public class StudentMarkViewDTO
    {
        public int MarkId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public double Mark { get; set; }
        public DateTime ExamDate { get; set; }
        public string TeacherName { get; set; }
    }
} 