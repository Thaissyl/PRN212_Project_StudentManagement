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
            string password = string.Empty;

            if (parameter is PasswordBox passwordBox)
            {
                password = passwordBox.Password;
            }
            else if (parameter is string p)
            {
                password = p;
            }

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Email and password cannot be empty.";
                return;
            }

            var user = _loginService.Authenticate(Email, password);

            if (user != null)
            {
                if (!user.IsActive)
                {
                    ErrorMessage = "Your account is inactive. Please contact support.";
                    return;
                }

                switch (user.Role)
                {
                    case "Teacher":
                    case "Student":
                    case "Manager":
                        if (Application.Current != null)
                        {
                            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

                            switch (user.Role)
                            {
                                case "Teacher":
                                    var mainStudentManagerView = new MainStudentManagerView(user);
                                    Application.Current.MainWindow = mainStudentManagerView;
                                    mainStudentManagerView.Show();
                                    break;
                                case "Student":
                                    var mainStudentView = new MainStudentView(user);
                                    Application.Current.MainWindow = mainStudentView;
                                    mainStudentView.Show();
                                    break;
                                case "Manager":
                                    var mainManagerView = new MainManagerView(user);
                                    Application.Current.MainWindow = mainManagerView;
                                    mainManagerView.Show();
                                    break;
                            }

                            currentWindow?.Close();
                        }
                        break;
                    default:
                        ErrorMessage = "Invalid role for this application.";
                        return;
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
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
