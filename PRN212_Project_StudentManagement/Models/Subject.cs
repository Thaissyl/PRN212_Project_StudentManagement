using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
