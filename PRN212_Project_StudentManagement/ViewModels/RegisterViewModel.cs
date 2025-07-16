using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private IUserRepository _userRepository;
        private User _newUser;
        private Window _window;

        public User NewUser
        {
            get { return _newUser; }
            set
            {
                _newUser = value;
                OnPropertyChanged(nameof(NewUser));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }
        public System.Collections.ObjectModel.ObservableCollection<string> Roles { get; set; }

        public RegisterViewModel(IUserRepository userRepository, Window window)
        {
            _userRepository = userRepository;
            _window = window;
            _newUser = new User();
            Roles = new System.Collections.ObjectModel.ObservableCollection<string> { "Student", "Teacher" };
            RegisterCommand = new ViewModelCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private bool CanExecuteRegisterCommand(object obj)
        {
            if (obj is System.Windows.Controls.PasswordBox passwordBox)
            {
                return !string.IsNullOrWhiteSpace(NewUser.FullName) &&
                       !string.IsNullOrWhiteSpace(NewUser.Email) &&
                       NewUser.Email.Contains("@gmail.com") &&
                       !string.IsNullOrWhiteSpace(passwordBox.Password) &&
                       passwordBox.Password.Length >= 8;
            }
            return false;
        }

        private void ExecuteRegisterCommand(object obj)
        {
            if (obj is System.Windows.Controls.PasswordBox passwordBox)
            {
                NewUser.Password = passwordBox.Password;
            }

            try
            {
                _userRepository.AddUser(NewUser);
                MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            _window.Close();
        }
    }
}
