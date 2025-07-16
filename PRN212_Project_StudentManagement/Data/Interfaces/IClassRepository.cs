using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface IClassRepository
    {
        IEnumerable<Class> GetAll();
    }
}
