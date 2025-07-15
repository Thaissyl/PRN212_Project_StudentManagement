using System;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
}
