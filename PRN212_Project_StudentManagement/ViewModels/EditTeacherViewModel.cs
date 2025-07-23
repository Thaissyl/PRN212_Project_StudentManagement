using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class EditTeacherViewModel : ViewModelBase
    {
        private int _teacherId;
        private int _userId;
        private string _fullName;
        private string _email;
        private int _selectedSubjectId;
        private DateTime _hireDate;
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

        public EditTeacherViewModel(Teacher teacher)
        {
            _teacherId = teacher.TeacherId;
            _userId = teacher.UserId;
            FullName = teacher.User.FullName;
            Email = teacher.User.Email;
            SelectedSubjectId = teacher.SubjectId;
            HireDate = teacher.HireDate.ToDateTime(TimeOnly.MinValue);
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
                // Update user
                var userRepo = new UserRepository();
                var user = userRepo.GetUserByEmail(Email);
                if (user == null) user = new User { UserId = _userId };
                user.FullName = this.FullName;
                user.Email = this.Email;
                userRepo.UpdateUser(user);

                // Update teacher
                var teacherRepo = new TeacherRepository();
                var teacher = teacherRepo.GetAllTeachers().Find(t => t.TeacherId == _teacherId);
                if (teacher != null)
                {
                    teacher.SubjectId = this.SelectedSubjectId;
                    teacher.HireDate = DateOnly.FromDateTime(this.HireDate);
                    // Cập nhật lại teacher
                    teacherRepo.UpdateTeacher(teacher);
                }

                MessageBox.Show("Teacher updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                (obj as Window)?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating teacher: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object obj)
        {
            (obj as Window)?.Close();
        }
    }
} 