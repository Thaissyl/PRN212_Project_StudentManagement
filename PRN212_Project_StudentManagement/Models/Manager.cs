using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public int UserId { get; set; }

    public DateOnly StartDate { get; set; }

    public virtual User User { get; set; } = null!;
}
