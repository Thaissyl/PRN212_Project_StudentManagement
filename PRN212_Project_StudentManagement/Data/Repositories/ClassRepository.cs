using PRN212_Project_StudentManagement.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public List<Class> GetAllClasses()
        {
            return _context.Classes
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t.Subject)
                .ToList();
        }

        public void UpdateClass(Class selectedClass)
        {
            _context.Classes.Update(selectedClass);
            _context.SaveChanges();
        }
    }
}
