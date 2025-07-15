using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class StudentLog
{
    public int LogId { get; set; }

    public int StudentId { get; set; }

    public string Action { get; set; } = null!;

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public int ChangeById { get; set; }

    public DateTime ActionDate { get; set; }

    public string? Description { get; set; }

    public virtual User ChangeBy { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
