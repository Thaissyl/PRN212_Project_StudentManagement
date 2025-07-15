using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<StudentLog> StudentLogs { get; set; } = new List<StudentLog>();

    public virtual User User { get; set; } = null!;
}
