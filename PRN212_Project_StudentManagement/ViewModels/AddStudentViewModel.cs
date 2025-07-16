using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.DTOs;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class AddStudentViewModel : ViewModelBase
    {
        private readonly IStudentRepository _studentRepository;
        private StudentDTO _newStudent;
        private Window _window;

        public StudentDTO NewStudent
        {
            get { return _newStudent; }
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }

        public ObservableCollection<string> ClassNames { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddStudentViewModel(Window window)
        {
            _studentRepository = new StudentRepository();
            _newStudent = new StudentDTO();
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
            LoadClassNames();
            _window = window;
        }

        private bool CanExecuteSaveCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(NewStudent.FullName) &&
                   !string.IsNullOrWhiteSpace(NewStudent.Email) &&
                   NewStudent.Email.Contains("@gmail.com") &&
                   !string.IsNullOrWhiteSpace(NewStudent.ClassName) &&
                   NewStudent.EnrollmentDateTime != default;
        }

        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                _studentRepository.AddStudent(NewStudent);
                MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _window.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            if (_window != null && _window.IsLoaded)
            {
                _window.Close();
            }
            else
            {
                MessageBox.Show("Unable to close the window. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadClassNames()
        {
            ClassNames = new ObservableCollection<string>(_studentRepository.GetAllClassNames());
        }
    }
}
