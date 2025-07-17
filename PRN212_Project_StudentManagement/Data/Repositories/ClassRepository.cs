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

        public void AddClass(Class @class)
        {
             _context.Classes.Add(@class);
            _context.SaveChanges();
        }

        public void DeleteClass(int classId)
        {
            _context.Classes.Remove(_context.Classes.Find(classId));
            _context.SaveChanges();
        }

        public IEnumerable<Class> GetAll()
        {
            return _context.Classes.ToList();
        }

        public void UpdateClass(Class selectedClass)
        {
            _context.Classes.Update(selectedClass);
            _context.SaveChanges();
        }
    }
}
