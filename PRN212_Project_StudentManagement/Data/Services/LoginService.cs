using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;

namespace PRN212_Project_StudentManagement.Data.Services
{
    internal class LoginService : ILoginService
    {
        public User Authenticate(string email, string password)
        {
            using (var context = new DBContext())
            {
                return context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            }
        }
    }
}
