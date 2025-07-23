using Xunit;
using Moq;
using PRN212_Project_StudentManagement.ViewModels;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Data.Interfaces;
using System.Windows;

public class EditProfileViewModelTests
{
    [Fact]
    public void CanExecuteSaveCommand_ReturnsFalse_WhenPasswordIsNullOrShort()
    {
        var userRepo = new Mock<IUserRepository>();
        var user = new User { FullName = "Test", Email = "test@gmail.com", Password = "1234567" };
        var window = new Mock<Window>();
        var vm = new EditProfileViewModel(userRepo.Object, user, window.Object);
        vm.Password = "1234567";
        var canExecute = vm.SaveCommand.CanExecute(null);
        Assert.False(canExecute);
        vm.Password = "12345678";
        canExecute = vm.SaveCommand.CanExecute(null);
        Assert.True(canExecute);
    }

    [Fact]
    public void PropertyChanged_Raised_WhenFullNameChanges()
    {
        var userRepo = new Mock<IUserRepository>();
        var user = new User { FullName = "Test", Email = "test@gmail.com", Password = "12345678" };
        var window = new Mock<Window>();
        var vm = new EditProfileViewModel(userRepo.Object, user, window.Object);
        bool raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == "FullName") raised = true; };
        vm.FullName = "New Name";
        Assert.True(raised);
    }

    [Fact]
    public void ExecuteSaveCommand_CallsUpdateUserAndCloseWindow()
    {
        var userRepo = new Mock<IUserRepository>();
        var user = new User { FullName = "Test", Email = "test@gmail.com", Password = "12345678" };
        var window = new Mock<Window>();
        var vm = new EditProfileViewModel(userRepo.Object, user, window.Object);
        vm.FullName = "New Name";
        vm.Email = "new@gmail.com";
        vm.Password = "abcdefgh";
        vm.SaveCommand.Execute(null);
        userRepo.Verify(r => r.UpdateUser(It.Is<User>(u => u.FullName == "New Name" && u.Email == "new@gmail.com" && u.Password == "abcdefgh")), Times.Once);
        window.Verify(w => w.Close(), Times.Once);
    }
} 