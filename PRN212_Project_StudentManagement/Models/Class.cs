using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int? TeacherId { get; set; }

    public int AcademicFromYear { get; set; }
    public int AcademicToYear { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual Teacher? Teacher { get; set; }
}
