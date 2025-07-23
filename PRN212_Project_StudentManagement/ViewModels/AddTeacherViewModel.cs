using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class AddTeacherViewModel : ViewModelBase
    {
        private string _fullName;
        private string _email;
        private int _selectedSubjectId;
        private DateTime _hireDate = DateTime.Now;
        private ObservableCollection<Subject> _subjects;

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public int SelectedSubjectId
        {
            get => _selectedSubjectId;
            set { _selectedSubjectId = value; OnPropertyChanged(nameof(SelectedSubjectId)); }
        }
        public DateTime HireDate
        {
            get => _hireDate;
            set { _hireDate = value; OnPropertyChanged(nameof(HireDate)); }
        }
        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(nameof(Subjects)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddTeacherViewModel()
        {
            Subjects = new ObservableCollection<Subject>(new SubjectRepository().GetAllSubjects());
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }

        private bool CanExecuteSaveCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(FullName) && !string.IsNullOrWhiteSpace(Email) && Email.EndsWith("@gmail.com") && SelectedSubjectId > 0;
        }

        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                // Tạo user mới
                var user = new User
                {
                    FullName = this.FullName,
                    Email = this.Email,
                    Password = "12345678", // default password
                    Role = "Teacher",
                    IsActive = true
                };
                var userRepo = new UserRepository();
                userRepo.AddUser(user);

                // Lấy lại user vừa tạo
                var createdUser = userRepo.GetUserByEmail(this.Email);

                // Tạo teacher mới
                var teacher = new Teacher
                {
                    UserId = createdUser.UserId,
                    SubjectId = this.SelectedSubjectId,
                    HireDate = DateOnly.FromDateTime(this.HireDate)
                };
                var teacherRepo = new TeacherRepository();
                teacherRepo.AddTeacher(teacher);

                MessageBox.Show("Teacher added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                (obj as Window)?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding teacher: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            (obj as Window)?.Close();
        }
    }
} 