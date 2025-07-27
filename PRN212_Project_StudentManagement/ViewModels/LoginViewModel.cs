using PRN212_Project_StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using PRN212_Project_StudentManagement.Views;
using System.Windows.Controls;
using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Services;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;

        private string _email;
        private string _errorMessage;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand);
            ErrorMessage = string.Empty;
            Email = string.Empty;
        }

        private void ExecuteLoginCommand(object parameter)
        {
            string password = parameter as string;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Email and password cannot be empty.";
                return;
            }

            var user = _loginService.Authenticate(Email, password);

            if (user != null)
            {
                if (user.Role == "Student" || user.Role == "Teacher" || user.Role == "Manager")
                {
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Invalid role for this application.";
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
            }
        }
    }

    public interface ILoginService
    {
        User Authenticate(string email, string password);
    }
}
