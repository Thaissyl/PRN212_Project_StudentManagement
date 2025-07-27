using System;

namespace PRN212_Project_StudentManagement.DTOs
{
    public class StudentMarkDTO
    {
        public int MarkId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public double Mark1 { get; set; }
        public DateOnly ExamDate { get; set; }
    }
} 