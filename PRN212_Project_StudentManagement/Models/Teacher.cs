using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int UserId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly HireDate { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
