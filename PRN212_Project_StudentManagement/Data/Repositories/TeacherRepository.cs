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
            return _context.Teachers.Include(t => t.User).Include(t => t.Subject).ToList();
        }

        public void AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
        }

        public void DeleteTeacher(int teacherId)
        {
            var teacher = _context.Teachers.Find(teacherId);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }

        public void UpdateTeacher(Teacher teacher)
        {
            _context.Teachers.Update(teacher);
            _context.SaveChanges();
        }
    }
}
