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
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand);
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand);
            ForgotPasswordCommand = new ViewModelCommand(ExecuteForgotPasswordCommand);

            // Clear any previous error messages
            ErrorMessage = string.Empty;
            Email = string.Empty;
        }

        private void ExecuteLoginCommand(object parameter)
        {
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;

                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
                {
                    ErrorMessage = "Email and password cannot be empty.";
                    return;
                }

                var user = _loginService.Authenticate(Email, password);

                if (user != null)
                {
                    Window currentWindow = Application.Current.MainWindow;
                    if (user.Role == "Teacher")
                    {
                        var mainView = new MainStudentManagerView(user);
                        Application.Current.MainWindow = mainView;
                        mainView.Show();
                        currentWindow.Close();
                    }
                    else if (user.Role == "Student")
                    {
                        var mainView = new MainStudentView(user);
                        Application.Current.MainWindow = mainView;
                        mainView.Show();
                        currentWindow.Close();
                    }
                    else if (user.Role == "Manager")
                    {
                        var mainView = new MainManagerView(user);
                        Application.Current.MainWindow = mainView;
                        mainView.Show();
                        currentWindow.Close();
                    }
                    else
                    {
                        ErrorMessage = "Invalid role for this application";
                    }
                }
                else
                {
                    ErrorMessage = "Invalid email or password";
                }
            }
        }

        private void ExecuteRegisterCommand(object obj)
        {
            var registerView = new RegisterView();
            registerView.Show();
        }

        private void ExecuteForgotPasswordCommand(object obj)
        {
            var forgotPasswordView = new ForgotPasswordView();
            forgotPasswordView.Show();
        }
    }
}
