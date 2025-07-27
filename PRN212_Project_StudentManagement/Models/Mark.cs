using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Mark
{
    public int MarkId { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public int ClassId { get; set; }

    public int SemesterId { get; set; }

    public double Mark1 { get; set; }

    public DateOnly ExamDate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Semester Semester { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
