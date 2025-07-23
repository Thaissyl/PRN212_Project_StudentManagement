using Xunit;
using PRN212_Project_StudentManagement.ViewModels;
using System.Collections.ObjectModel;

public class AddClassViewModelTests
{
    [Fact]
    public void CanExecuteSaveCommand_ReturnsFalse_WhenInvalidInput()
    {
        var vm = new AddClassViewModel();
        vm.ClassName = "";
        vm.SelectedTeacherId = 0;
        vm.AcademicFromYear = 2024;
        vm.AcademicToYear = 2023;
        var canExecute = vm.SaveCommand.CanExecute(null);
        Assert.False(canExecute);
        vm.ClassName = "Math";
        vm.SelectedTeacherId = 1;
        vm.AcademicFromYear = 2023;
        vm.AcademicToYear = 2024;
        canExecute = vm.SaveCommand.CanExecute(null);
        Assert.True(canExecute);
    }

    [Fact]
    public void PropertyChanged_Raised_WhenClassNameChanges()
    {
        var vm = new AddClassViewModel();
        bool raised = false;
        vm.PropertyChanged += (s, e) => { if (e.PropertyName == "ClassName") raised = true; };
        vm.ClassName = "NewClass";
        Assert.True(raised);
    }
} 