using PRN212_Project_StudentManagement.Models;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUserByEmail(string email);
        void UpdateUser(User user);
    }
}
