using Xunit;
using Moq;
using PRN212_Project_StudentManagement.ViewModels;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Data.Interfaces;
using System.Windows;
using System.Collections.ObjectModel;

public class RegisterViewModelTests
{
    [Fact]
    public void CanExecuteRegisterCommand_ReturnsFalse_WhenInvalidInput()
    {
        var userRepo = new Mock<IUserRepository>();
        var window = new Mock<Window>();
        var vm = new RegisterViewModel(userRepo.Object, window.Object);
        vm.NewUser.FullName = "";
        vm.NewUser.Email = "notgmail";
        var passwordBox = new System.Windows.Controls.PasswordBox { Password = "123" };
        var canExecute = vm.RegisterCommand.CanExecute(passwordBox);
        Assert.False(canExecute);
        vm.NewUser.FullName = "Test";
        vm.NewUser.Email = "test@gmail.com";
        passwordBox.Password = "12345678";
        canExecute = vm.RegisterCommand.CanExecute(passwordBox);
        Assert.True(canExecute);
    }

    [Fact]
    public void PropertyChanged_Raised_WhenNewUserChanges()
    {
        var userRepo = new Mock<IUserRepository>();
        var window = new Mock<Window>();
        var vm = new RegisterViewModel(userRepo.Object, window.Object);
        bool raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == "NewUser") raised = true; };
        vm.NewUser = new User();
        Assert.True(raised);
    }
} 