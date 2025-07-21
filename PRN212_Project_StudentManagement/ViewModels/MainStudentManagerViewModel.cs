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
        private readonly IClassRepository _classRepository;
        private ObservableCollection<StudentDTO> _students;
        private ObservableCollection<StudentDTO> _allStudents; // Store all students for filtering
        private StudentDTO _selectedStudent;
        private string _searchName;
        private string _searchStudentId;
        private string _selectedClassFilter;
        private User _currentUser;
        private bool _isAddingNewStudent;
        private StudentDTO _newStudent;
        private ObservableCollection<StudentDTO> _unassignedStudents;
        private StudentDTO _selectedUnassignedStudent;
        private bool _isAssigningStudent;

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

        public string SelectedClassFilter
        {
            get { return _selectedClassFilter; }
            set
            {
                _selectedClassFilter = value;
                OnPropertyChanged(nameof(SelectedClassFilter));
                ExecuteSearchCommand(null);
            }
        }

        public ObservableCollection<string> ClassNames { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand NavigateToClassViewCommand { get; }
        public ICommand SaveNewStudentCommand { get; }
        public ICommand CancelAddCommand { get; }
        public ICommand AssignStudentCommand { get; }
        public ICommand CancelAssignCommand { get; }
        public ICommand AssignCommand { get; }

        public string TeacherFullName
        {
            get { return _currentUser?.FullName; }
        }

        public bool IsAddingNewStudent
        {
            get => _isAddingNewStudent;
            set
            {
                _isAddingNewStudent = value;
                OnPropertyChanged(nameof(IsAddingNewStudent));
                OnPropertyChanged(nameof(IsEditingStudent));
            }
        }

        public StudentDTO NewStudent
        {
            get => _newStudent;
            set
            {
                _newStudent = value;
                OnPropertyChanged(nameof(NewStudent));
            }
        }

        public bool IsEditingStudent
        {
            get => !_isAddingNewStudent && !_isAssigningStudent;
        }

        public ObservableCollection<StudentDTO> UnassignedStudents
        {
            get => _unassignedStudents;
            set
            {
                _unassignedStudents = value;
                OnPropertyChanged(nameof(UnassignedStudents));
            }
        }

        public StudentDTO SelectedUnassignedStudent
        {
            get => _selectedUnassignedStudent;
            set
            {
                _selectedUnassignedStudent = value;
                OnPropertyChanged(nameof(SelectedUnassignedStudent));
            }
        }

        public bool IsAssigningStudent
        {
            get => _isAssigningStudent;
            set
            {
                _isAssigningStudent = value;
                OnPropertyChanged(nameof(IsAssigningStudent));
                OnPropertyChanged(nameof(IsEditingStudent));
            }
        }

        public MainStudentManagerViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _studentRepository = new StudentRepository();
            _classRepository = new ClassRepository();
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);
            EditProfileCommand = new ViewModelCommand(ExecuteEditProfileCommand);
            UpdateCommand = new ViewModelCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);
            ResetCommand = new ViewModelCommand(ExecuteResetCommand);
            AddCommand = new ViewModelCommand(ExecuteAddCommand); // Initialize AddCommand
            DeleteCommand = new ViewModelCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            NavigateToClassViewCommand = new ViewModelCommand(ExecuteNavigateToClassViewCommand);
            SaveNewStudentCommand = new ViewModelCommand(ExecuteSaveNewStudentCommand, CanExecuteSaveNewStudentCommand);
            CancelAddCommand = new ViewModelCommand(ExecuteCancelAddCommand);
            AssignStudentCommand = new ViewModelCommand(ExecuteAssignStudentCommand, CanExecuteAssignStudentCommand);
            CancelAssignCommand = new ViewModelCommand(ExecuteCancelAssignCommand);
            AssignCommand = new ViewModelCommand(ExecuteAssignCommand);
            NewStudent = new StudentDTO();
            UnassignedStudents = new ObservableCollection<StudentDTO>();
            LoadStudents();
            LoadClassNames();
            LoadUnassignedStudents();
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
            var classNames = _classRepository.GetAllClasses().Select(c => c.ClassName).ToList();
            classNames.Insert(0, "All Classes");
            ClassNames = new ObservableCollection<string>(classNames);
            SelectedClassFilter = "All Classes";
        }

        private void ExecuteLogoutCommand(object obj)
        {
            var loginView = new LoginView();
            Application.Current.MainWindow = loginView;
            loginView.Show();
            
            // Close current window
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteSearchCommand(object obj)
        {
            DebugSearch("Executing search with SearchName: " + SearchName + ", SearchStudentId: " + SearchStudentId);
            var filteredStudents = _allStudents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                filteredStudents = filteredStudents.Where(s => s.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SearchStudentId))
            {
                filteredStudents = filteredStudents.Where(s => s.StudentID.ToString().Contains(SearchStudentId, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedClassFilter) && SelectedClassFilter != "All Classes")
            {
                filteredStudents = filteredStudents.Where(s => s.ClassName == SelectedClassFilter);
            }

            Students = new ObservableCollection<StudentDTO>(filteredStudents.ToList());
            DebugSearch("Found " + Students.Count + " matching students.");
        }

        private void ExecuteResetCommand(object obj)
        {
            SearchName = string.Empty;
            SearchStudentId = string.Empty;
            SelectedClassFilter = "All Classes";
            SelectedStudent = null; // Reset the selected student to clear details
            Students = new ObservableCollection<StudentDTO>(_allStudents);
            DebugSearch("Reset applied, showing all students and cleared details.");
        }

        private void ExecuteAddCommand(object obj)
        {
            // Clear the details section and enter add mode
            IsAddingNewStudent = true;
            NewStudent = new StudentDTO();
            SelectedStudent = null; // Clear any selected student
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

        private void ExecuteNavigateToClassViewCommand(object obj)
        {
            var mainClassView = new MainClassView(_currentUser);
            mainClassView.Show();
            Application.Current.MainWindow.Close();
        }

        private bool CanExecuteSaveNewStudentCommand(object obj)
        {
            return IsAddingNewStudent &&
                   !string.IsNullOrWhiteSpace(NewStudent.FullName) &&
                   !string.IsNullOrWhiteSpace(NewStudent.Email) &&
                   NewStudent.Email.Contains("@gmail.com") &&
                   !string.IsNullOrWhiteSpace(NewStudent.ClassName) &&
                   NewStudent.EnrollmentDateTime != default;
        }

        private void ExecuteSaveNewStudentCommand(object obj)
        {
            try
            {
                _studentRepository.AddStudent(NewStudent);
                MessageBox.Show("Student added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Exit add mode and refresh the list
                IsAddingNewStudent = false;
                NewStudent = new StudentDTO();
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelAddCommand(object obj)
        {
            // Exit add mode and clear the details
            IsAddingNewStudent = false;
            NewStudent = new StudentDTO();
            SelectedStudent = null;
        }

        private void LoadUnassignedStudents()
        {
            var unassignedStudents = _studentRepository.GetUnassignedStudents();
            UnassignedStudents = new ObservableCollection<StudentDTO>(unassignedStudents);
        }

        private bool CanExecuteAssignStudentCommand(object obj)
        {
            return IsAssigningStudent && 
                   SelectedUnassignedStudent != null && 
                   !string.IsNullOrWhiteSpace(SelectedUnassignedStudent.ClassName) &&
                   SelectedUnassignedStudent.ClassName != "No Class Assigned";
        }

        private void ExecuteAssignStudentCommand(object obj)
        {
            try
            {
                _studentRepository.AssignStudentToClass(SelectedUnassignedStudent.StudentID, SelectedUnassignedStudent.ClassName);
                MessageBox.Show("Student assigned to class successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Exit assign mode and refresh lists
                IsAssigningStudent = false;
                SelectedUnassignedStudent = null;
                LoadStudents();
                LoadUnassignedStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning student to class: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelAssignCommand(object obj)
        {
            // Exit assign mode and clear the details
            IsAssigningStudent = false;
            IsAddingNewStudent = false; // Return to normal state
            SelectedUnassignedStudent = null;
            SelectedStudent = null;
        }

        private void ExecuteAssignCommand(object obj)
        {
            // Check if an unassigned student is selected
            if (SelectedUnassignedStudent == null)
            {
                MessageBox.Show("Please select an unassigned student first.", "No Student Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Exit add mode and enter assign mode
            IsAddingNewStudent = false;
            IsAssigningStudent = true;
            SelectedStudent = null; // Clear any selected student
        }
    }
}
