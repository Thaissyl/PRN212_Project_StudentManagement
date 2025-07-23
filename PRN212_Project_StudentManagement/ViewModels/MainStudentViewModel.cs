using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.DTOs;
using System;
using System.Collections.Generic;
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

        public ICommand LogoutCommand { get; }
        public ICommand ViewProfileCommand { get; }

        public MainStudentViewModel(User user)
        {
            CurrentUser = user;
            WelcomeMessage = $"Welcome, {user.FullName}!";
            StudentClasses = new List<StudentClassDTO>();
            
            LogoutCommand = new ViewModelCommand(ExecuteLogoutCommand);
            ViewProfileCommand = new ViewModelCommand(ExecuteViewProfileCommand);
            
            LoadStudentClasses();
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
    }
} 