using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System.Linq;

namespace PRN212_Project_StudentManagement.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void AddUser(User user)
        {
            using (var context = new DBContext())
            {
                user.IsActive = true;
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new DBContext())
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public void UpdateUser(User user)
        {
            using (var context = new DBContext())
            {
                context.Users.Update(user);
                context.SaveChanges();
            }
        }
    }
}
