using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace PRN212_Project_StudentManagement.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly DBContext _context;

        public TeacherRepository()
        {
            _context = new DBContext();
        }

        public List<Teacher> GetAllTeachers()
        {
            return _context.Teachers.Include(t => t.User).ToList();
        }
    }
}
