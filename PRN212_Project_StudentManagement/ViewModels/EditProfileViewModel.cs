using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly User _user;
        private readonly Window _window;

        private string _fullName;
        private string _email;
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditProfileViewModel(IUserRepository userRepository, User user, Window window)
        {
            _userRepository = userRepository;
            _user = user;
            _window = window;

            FullName = user.FullName;
            Email = user.Email;
            Password = user.Password;

            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private bool CanExecuteSaveCommand(object obj)
        {
            return !string.IsNullOrEmpty(Password) && Password.Length >= 8;
        }

        private void ExecuteSaveCommand(object obj)
        {
            _user.FullName = FullName;
            _user.Email = Email;
            _user.Password = Password;
            _userRepository.UpdateUser(_user);
            _window.Close();
        }

        private void ExecuteCancelCommand(object obj)
        {
            _window.Close();
        }
    }
}
