using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public int UserId { get; set; }

    public int DepartmentId { get; set; }

    public DateOnly HireDate { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
