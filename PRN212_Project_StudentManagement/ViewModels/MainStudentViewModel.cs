using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class MainStudentViewModel : ViewModelBase
    {
        private User _currentUser;
        private string _welcomeMessage;
        private List<StudentClassDTO> _studentClasses;
        private bool _hasClasses;
        private ObservableCollection<StudentMarkViewDTO> _studentMarks;
        private StudentMarkViewDTO _selectedMark;
        private double _averageMarkSemester1;
        private double _averageMarkSemester2;
        private double _yearlyAverage;
        private string _academicPerformance;
        private bool _isEditingProfile;
        private string _editFullName;
        private string _editEmail;
        private ObservableCollection<string> _semesterFilters;
        private string _selectedSemesterFilter;
        private ObservableCollection<StudentMarkViewDTO> _filteredStudentMarks;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        public List<StudentClassDTO> StudentClasses
        {
            get => _studentClasses;
            set
            {
                _studentClasses = value;
                OnPropertyChanged(nameof(StudentClasses));
                HasClasses = value != null && value.Count > 0;
            }
        }

        public bool HasClasses
        {
            get => _hasClasses;
            set
            {
                _hasClasses = value;
                OnPropertyChanged(nameof(HasClasses));
                OnPropertyChanged(nameof(ShowNoClassesMessage));
            }
        }

        public bool ShowNoClassesMessage
        {
            get => !_hasClasses;
        }

        public ObservableCollection<string> SemesterFilters
        {
            get => _semesterFilters;
            set
            {
                _semesterFilters = value;
                OnPropertyChanged(nameof(SemesterFilters));
            }
        }

        public string SelectedSemesterFilter
        {
            get => _selectedSemesterFilter;
            set
            {
                _selectedSemesterFilter = value;
                OnPropertyChanged(nameof(SelectedSemesterFilter));
                ApplySemesterFilter();
            }
        }

        public ObservableCollection<StudentMarkViewDTO> FilteredStudentMarks
        {
            get => _filteredStudentMarks;
            set
            {
                _filteredStudentMarks = value;
                OnPropertyChanged(nameof(FilteredStudentMarks));
            }
        }

        public ObservableCollection<StudentMarkViewDTO> StudentMarks
        {
            get => _studentMarks;
            set
            {
                _studentMarks = value;
                OnPropertyChanged(nameof(StudentMarks));
            }
        }

        public StudentMarkViewDTO SelectedMark
        {
            get => _selectedMark;
            set
            {
                _selectedMark = value;
                OnPropertyChanged(nameof(SelectedMark));
            }
        }

        public double AverageMarkSemester1
        {
            get => _averageMarkSemester1;
            set
            {
                _averageMarkSemester1 = value;
                OnPropertyChanged(nameof(AverageMarkSemester1));
            }
        }

        public double AverageMarkSemester2
        {
            get => _averageMarkSemester2;
            set
            {
                _averageMarkSemester2 = value;
                OnPropertyChanged(nameof(AverageMarkSemester2));
            }
        }

        public double YearlyAverage
        {
            get => _yearlyAverage;
            set
            {
                _yearlyAverage = value;
                OnPropertyChanged(nameof(YearlyAverage));
            }
        }

        public string AcademicPerformance
        {
            get => _academicPerformance;
            set
            {
                _academicPerformance = value;
                OnPropertyChanged(nameof(AcademicPerformance));
            }
        }

        public bool IsEditingProfile
        {
            get => _isEditingProfile;
            set
            {
                _isEditingProfile = value;
                OnPropertyChanged(nameof(IsEditingProfile));
                if (value)
                {
                    EditFullName = CurrentUser.FullName;
                    EditEmail = CurrentUser.Email;
                }
            }
        }

        public string EditFullName
        {
            get => _editFullName;
            set
            {
                _editFullName = value;
                OnPropertyChanged(nameof(EditFullName));
            }
        }

        public string EditEmail
        {
            get => _editEmail;
            set
            {
                _editEmail = value;
                OnPropertyChanged(nameof(EditEmail));
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public MainStudentViewModel(User user)
        {
            CurrentUser = user;
            WelcomeMessage = $"Welcome, {user.FullName}!";
            StudentClasses = new List<StudentClassDTO>();
            StudentMarks = new ObservableCollection<StudentMarkViewDTO>();
            FilteredStudentMarks = new ObservableCollection<StudentMarkViewDTO>();
            SemesterFilters = new ObservableCollection<string>();
            
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);
            ViewProfileCommand = new ViewModelCommand(ExecuteViewProfileCommand);
            SaveProfileCommand = new ViewModelCommand(ExecuteSaveProfileCommand, CanExecuteSaveProfileCommand);
            CancelEditCommand = new ViewModelCommand(ExecuteCancelEditCommand);
            EditProfileCommand = new ViewModelCommand(ExecuteEditProfileCommand);
            ClearFilterCommand = new ViewModelCommand(ExecuteClearFilterCommand);
            
            LoadStudentClasses();
            LoadStudentMarks();
            CalculateAcademicPerformance();
        }

        private void ExecuteLogoutCommand(object obj)
        {
            var loginView = new Views.LoginView();
            Application.Current.MainWindow = loginView;
            loginView.Show();
            
            // Close current window
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteViewProfileCommand(object obj)
        {
            var editProfileView = new Views.EditProfileView(CurrentUser);
            editProfileView.Show();
        }

        private void ExecuteEditProfileCommand(object obj)
        {
            IsEditingProfile = true;
        }

        private bool CanExecuteSaveProfileCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(EditFullName) && 
                   !string.IsNullOrWhiteSpace(EditEmail) && 
                   EditEmail.Contains("@");
        }

        private void ExecuteSaveProfileCommand(object obj)
        {
            try
            {
                using (var context = new DBContext())
                {
                    var user = context.Users.Find(CurrentUser.UserId);
                    if (user != null)
                    {
                        user.FullName = EditFullName;
                        user.Email = EditEmail;
                        context.SaveChanges();
                        
                        CurrentUser.FullName = EditFullName;
                        CurrentUser.Email = EditEmail;
                        WelcomeMessage = $"Welcome, {EditFullName}!";
                        
                        IsEditingProfile = false;
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelEditCommand(object obj)
        {
            IsEditingProfile = false;
        }

        private void ExecuteClearFilterCommand(object obj)
        {
            SelectedSemesterFilter = "All Semesters";
        }

        private void LoadStudentClasses()
        {
            try
            {
                using (var context = new DBContext())
                {
                    var studentClasses = context.Students
                        .Where(s => s.UserId == CurrentUser.UserId)
                        .Select(s => new StudentClassDTO
                        {
                            ClassId = s.ClassId,
                            ClassName = s.Class.ClassName,
                            AcademicFromYear = s.Class.AcademicFromYear,
                            AcademicToYear = s.Class.AcademicToYear,
                            TeacherName = s.Class.Teacher != null ? s.Class.Teacher.User.FullName : "No Teacher Assigned",
                            SubjectName = s.Class.Teacher != null ? s.Class.Teacher.Subject.SubjectName : "No Subject Assigned",
                            EnrollmentDate = s.EnrollmentDate
                        })
                        .ToList();

                    StudentClasses = studentClasses;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student classes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStudentMarks()
        {
            try
            {
                using (var context = new DBContext())
                {
                    // Get student ID from the current user
                    var student = context.Students.FirstOrDefault(s => s.UserId == CurrentUser.UserId);
                    if (student != null)
                    {
                        var studentMarks = context.Marks
                            .Where(m => m.StudentId == student.StudentId && m.SemesterId != 3)
                            .Select(m => new StudentMarkViewDTO
                            {
                                MarkId = m.MarkId,
                                StudentId = m.StudentId,
                                StudentName = m.Student.User.FullName,
                                Email = m.Student.User.Email,
                                ClassName = m.Class.ClassName,
                                SubjectId = m.SubjectId,
                                SubjectName = m.Subject.SubjectName,
                                SemesterId = m.SemesterId,
                                SemesterName = m.Semester.SemesterName,
                                Mark = m.Mark1,
                                ExamDate = m.ExamDate.ToDateTime(TimeOnly.MinValue),
                                TeacherName = m.Class.Teacher != null ? m.Class.Teacher.User.FullName : "No Teacher Assigned"
                            })
                            .ToList();

                        StudentMarks = new ObservableCollection<StudentMarkViewDTO>(studentMarks);
                        FilteredStudentMarks = new ObservableCollection<StudentMarkViewDTO>(studentMarks);
                        
                        // Load semester filters
                        var semesters = studentMarks.Select(m => m.SemesterName).Distinct().OrderBy(s => s).ToList();
                        semesters.Insert(0, "All Semesters");
                        SemesterFilters = new ObservableCollection<string>(semesters);
                        SelectedSemesterFilter = "All Semesters";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student marks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySemesterFilter()
        {
            if (string.IsNullOrEmpty(SelectedSemesterFilter) || SelectedSemesterFilter == "All Semesters")
            {
                FilteredStudentMarks = new ObservableCollection<StudentMarkViewDTO>(StudentMarks);
            }
            else
            {
                var filteredMarks = StudentMarks.Where(m => m.SemesterName == SelectedSemesterFilter).ToList();
                FilteredStudentMarks = new ObservableCollection<StudentMarkViewDTO>(filteredMarks);
            }
        }

        private void CalculateAcademicPerformance()
        {
            try
            {
                // Calculate semester 1 average
                var semester1Marks = StudentMarks.Where(m => m.SemesterId == 1).ToList();
                if (semester1Marks.Any())
                {
                    var semester1SubjectAverages = semester1Marks
                        .GroupBy(m => m.SubjectId)
                        .Select(g => g.Average(m => m.Mark))
                        .ToList();
                    AverageMarkSemester1 = semester1SubjectAverages.Average();
                }

                // Calculate semester 2 average
                var semester2Marks = StudentMarks.Where(m => m.SemesterId == 2).ToList();
                if (semester2Marks.Any())
                {
                    var semester2SubjectAverages = semester2Marks
                        .GroupBy(m => m.SubjectId)
                        .Select(g => g.Average(m => m.Mark))
                        .ToList();
                    AverageMarkSemester2 = semester2SubjectAverages.Average();
                }

                // Calculate yearly average: (Semester1 + Semester2 * 2) / 3
                YearlyAverage = (AverageMarkSemester1 + AverageMarkSemester2 * 2) / 3;

                // Determine academic performance
                AcademicPerformance = DetermineAcademicPerformance(YearlyAverage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calculating academic performance: {ex.Message}");
            }
        }

        private string DetermineAcademicPerformance(double yearlyAverage)
        {
            if (yearlyAverage >= 8.0)
                return "Giỏi";
            else if (yearlyAverage >= 6.5)
                return "Khá";
            else if (yearlyAverage >= 3.5)
                return "Trung Bình";
            else if (yearlyAverage >= 0)
                return "Yếu";
            else
                return "N/A";
        }
    }
} 