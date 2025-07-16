using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace PRN212_Project_StudentManagement.Data.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly DBContext _context;

        public ClassRepository()
        {
            _context = new DBContext();
        }

        public IEnumerable<Class> GetAll()
        {
            return _context.Classes.ToList();
        }
    }
}
