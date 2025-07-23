using PRN212_Project_StudentManagement.Models;
using Xunit;
using System.Collections.Generic;

namespace PRN212_Project_StudentManagement.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Initialization_DefaultValues()
        {
            // Arrange
            var user = new User();

            // Assert
            Assert.Equal(0, user.UserId);
            Assert.NotNull(user.FullName);
            Assert.NotNull(user.Email);
            Assert.NotNull(user.Password);
            Assert.NotNull(user.Role);
            Assert.False(user.IsActive);
            Assert.Null(user.Manager);
            Assert.Null(user.Student);
            Assert.NotNull(user.StudentLogs);
            Assert.IsType<List<StudentLog>>(user.StudentLogs);
            Assert.Null(user.Teacher);
        }
    }
} 