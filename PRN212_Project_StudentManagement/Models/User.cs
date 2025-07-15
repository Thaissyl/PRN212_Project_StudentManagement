using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual Student? Student { get; set; }

    public virtual ICollection<StudentLog> StudentLogs { get; set; } = new List<StudentLog>();

    public virtual Teacher? Teacher { get; set; }
}
