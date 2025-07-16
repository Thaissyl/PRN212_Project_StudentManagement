using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private IUserRepository _userRepository;
        private string _email;
        private string _password;
        private Window _window;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                (GetPasswordCommand as ViewModelCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand GetPasswordCommand { get; }
        public ICommand UpdatePasswordCommand { get; }
        public ICommand CancelCommand { get; }

        public ForgotPasswordViewModel(IUserRepository userRepository, Window window)
        {
            _userRepository = userRepository;
            _window = window;
            GetPasswordCommand = new ViewModelCommand(ExecuteGetPasswordCommand, CanExecuteGetPasswordCommand);
            UpdatePasswordCommand = new ViewModelCommand(ExecuteUpdatePasswordCommand, CanExecuteUpdatePasswordCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private bool CanExecuteGetPasswordCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Email) && Email.Contains("@gmail.com");
        }

        private void ExecuteGetPasswordCommand(object obj)
        {
            var user = _userRepository.GetUserByEmail(Email);
            if (user != null)
            {
                Password = user.Password;
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteUpdatePasswordCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Password) && Password.Length >= 8;
        }

        private void ExecuteUpdatePasswordCommand(object obj)
        {
            var user = _userRepository.GetUserByEmail(Email);
            if (user != null)
            {
                user.Password = Password;
                _userRepository.UpdateUser(user);
                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.Close();
            }
            else
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            _window.Close();
        }
    }
}
