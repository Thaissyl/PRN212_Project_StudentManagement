using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.DTOs;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class MainStudentManagerViewModel : ViewModelBase
    {
        private readonly IStudentRepository _studentRepository;
        private ObservableCollection<StudentDTO> _students;
        private ObservableCollection<StudentDTO> _allStudents; // Store all students for filtering
        private StudentDTO _selectedStudent;
        private string _searchName;
        private string _searchStudentId;
        private User _currentUser;

        public ICommand AddCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ObservableCollection<StudentDTO> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public StudentDTO SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                OnPropertyChanged(nameof(SearchName));
                ExecuteSearchCommand(null); // Trigger search on property change
                DebugSearch("SearchName updated to: " + _searchName);
            }
        }

        public string SearchStudentId
        {
            get { return _searchStudentId; }
            set
            {
                _searchStudentId = value;
                OnPropertyChanged(nameof(SearchStudentId));
                ExecuteSearchCommand(null); // Trigger search on property change
                DebugSearch("SearchStudentId updated to: " + _searchStudentId);
            }
        }

        public ObservableCollection<string> ClassNames { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DeleteCommand { get; }

        public string TeacherFullName
        {
            get { return _currentUser?.FullName; }
        }

        public MainStudentManagerViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _studentRepository = new StudentRepository();
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);
            EditProfileCommand = new ViewModelCommand(ExecuteEditProfileCommand);
            UpdateCommand = new ViewModelCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);
            ResetCommand = new ViewModelCommand(ExecuteResetCommand);
            AddCommand = new ViewModelCommand(ExecuteAddCommand); // Initialize AddCommand
            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            LoadStudents();
            LoadClassNames();
        }

        private bool CanExecuteUpdateCommand(object obj)
        {
            return SelectedStudent != null;
        }

        private bool CanExecuteDeleteCommand(object obj)
        {
            return SelectedStudent != null;
        }

        private void ExecuteUpdateCommand(object obj)
        {
            _studentRepository.UpdateStudent(SelectedStudent);
            LoadStudents();
        }

        private void ExecuteDeleteCommand(object obj)
        {
            if (MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _studentRepository.DeleteStudent(SelectedStudent.StudentID);
                LoadStudents();
            }
        }

        private void LoadStudents()
        {
            _allStudents = new ObservableCollection<StudentDTO>(_studentRepository.GetAllStudents());
            Students = new ObservableCollection<StudentDTO>(_allStudents);
            DebugSearch("Loaded " + _allStudents.Count + " students.");
        }

        private void LoadClassNames()
        {
            ClassNames = new ObservableCollection<string>(_studentRepository.GetAllClassNames());
        }

        private void ExecuteLogoutCommand(object obj)
        {
            var loginView = new LoginView();
            loginView.Show();
            Application.Current.MainWindow.Close();
        }

        private void ExecuteSearchCommand(object obj)
        {
            DebugSearch("Executing search with SearchName: " + SearchName + ", SearchStudentId: " + SearchStudentId);
            if (string.IsNullOrWhiteSpace(SearchName) && string.IsNullOrWhiteSpace(SearchStudentId))
            {
                Students = new ObservableCollection<StudentDTO>(_allStudents);
                DebugSearch("No search criteria, showing all students.");
                return;
            }

            var filteredStudents = _allStudents.Where(student =>
            {
                bool matchesName = string.IsNullOrWhiteSpace(SearchName) ||
                    (!string.IsNullOrEmpty(student.FullName) && student.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase));
                bool matchesStudentId = string.IsNullOrWhiteSpace(SearchStudentId) ||
                    student.StudentID.ToString().Contains(SearchStudentId, StringComparison.Ordinal);
                return matchesName && matchesStudentId;
            }).ToList();

            Students = new ObservableCollection<StudentDTO>(filteredStudents);
            DebugSearch("Found " + filteredStudents.Count + " matching students.");
        }

        private void ExecuteResetCommand(object obj)
        {
            SearchName = string.Empty;
            SearchStudentId = string.Empty;
            SelectedStudent = null; // Reset the selected student to clear details
            Students = new ObservableCollection<StudentDTO>(_allStudents);
            DebugSearch("Reset applied, showing all students and cleared details.");
        }

        private void ExecuteAddCommand(object obj)
        {
            var addStudentView = new AddStudentView();
            addStudentView.ShowDialog();
            LoadStudents(); // Refresh the student list after adding
        }

        private void ExecuteEditProfileCommand(object obj)
        {
            var editProfileView = new EditProfileView(_currentUser);
            editProfileView.ShowDialog();
            OnPropertyChanged(nameof(TeacherFullName));
        }

        private void DebugSearch(string message)
        {
            System.Diagnostics.Debug.WriteLine(message); // Use this in Visual Studio Output window
        }
    }
}
