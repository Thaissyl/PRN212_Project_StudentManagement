using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Semester
{
    public int SemesterId { get; set; }

    public string SemesterName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();
}
