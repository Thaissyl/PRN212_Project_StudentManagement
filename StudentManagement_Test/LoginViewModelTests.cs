using Xunit;
using Moq;
using PRN212_Project_StudentManagement.ViewModels;
using PRN212_Project_StudentManagement.Models;

namespace StudentManagement_Test
{
    public class LoginViewModelTests
    {
        [Fact]
        public void Login_WithEmptyEmailAndPassword_ShouldReturnError()
        {
            var mockService = new Mock<ILoginService>();
            var viewModel = new LoginViewModel(mockService.Object)
            {
                Email = ""
            };

            viewModel.LoginCommand.Execute("");

            Assert.Equal("Email and password cannot be empty.", viewModel.ErrorMessage);
        }

        [Fact]
        public void Login_WithIncorrectCredentials_ShouldReturnError()
        {
            var mockService = new Mock<ILoginService>();
            mockService.Setup(s => s.Authenticate("wrong@gmail.com", "wrongpass")).Returns((User)null);

            var viewModel = new LoginViewModel(mockService.Object)
            {
                Email = "wrong@gmail.com"
            };

            viewModel.LoginCommand.Execute("wrongpass");

            Assert.Equal("Invalid email or password.", viewModel.ErrorMessage);
        }

        [Theory]
        [InlineData("binh.tt@school.com")]
        [InlineData("mai.ht@school.com")]
        [InlineData("hung.nv@school.com")]
        [InlineData("lan.lt@school.com")]
        [InlineData("tuan.dm@school.com")]
        [InlineData("ngoc.pt@school.com")]
        [InlineData("long.tv@school.com")]
        [InlineData("ada@gmail.com")]
        [InlineData("hiep@gmail.com")]
        public void Login_WithValidStudentAccount_ShouldSucceed(string email)
        {
            var mockService = new Mock<ILoginService>();
            mockService.Setup(s => s.Authenticate(email, "12345678"))
                .Returns(new User { Email = email, Role = "Student", IsActive = true });

            var viewModel = new LoginViewModel(mockService.Object)
            {
                Email = email
            };

            viewModel.LoginCommand.Execute("12345678");

            Assert.Equal(string.Empty, viewModel.ErrorMessage);
        }

        [Fact]
        public void Login_WithInvalidRole_ShouldReturnRoleError()
        {
            var mockService = new Mock<ILoginService>();
            mockService.Setup(s => s.Authenticate("weird@school.com", "12345678"))
                .Returns(new User { Email = "weird@school.com", Role = "Alien", IsActive = true });

            var viewModel = new LoginViewModel(mockService.Object)
            {
                Email = "weird@school.com"
            };

            viewModel.LoginCommand.Execute("12345678");

            Assert.Equal("Invalid role for this application.", viewModel.ErrorMessage);
        }
    }
}
