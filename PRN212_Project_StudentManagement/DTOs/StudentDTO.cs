using System;

namespace PRN212_Project_StudentManagement.DTOs
{
    public class StudentDTO
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string Email { get; set; }
        public DateOnly EnrollmentDate { get; set; }

        public DateTime EnrollmentDateTime
        {
            get => EnrollmentDate.ToDateTime(TimeOnly.MinValue);
            set => EnrollmentDate = DateOnly.FromDateTime(value);
        }
    }
}
