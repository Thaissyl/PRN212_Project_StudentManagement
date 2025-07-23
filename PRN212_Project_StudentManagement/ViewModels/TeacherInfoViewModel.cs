using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class TeacherInfoViewModel : ViewModelBase
    {
        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set { _teachers = value; OnPropertyChanged(nameof(Teachers)); }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set { _selectedTeacher = value; OnPropertyChanged(nameof(SelectedTeacher)); }
        }
        public ICommand AddTeacherCommand { get; }
        public ICommand EditTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }

        public TeacherInfoViewModel()
        {
            Teachers = new ObservableCollection<Teacher>(new TeacherRepository().GetAllTeachers());
            AddTeacherCommand = new ViewModelCommand(ExecuteAddTeacherCommand);
            EditTeacherCommand = new ViewModelCommand(ExecuteEditTeacherCommand, CanExecuteEditOrDeleteTeacher);
            DeleteTeacherCommand = new ViewModelCommand(ExecuteDeleteTeacherCommand, CanExecuteEditOrDeleteTeacher);
        }

        private void ExecuteAddTeacherCommand(object obj)
        {
            var addTeacherView = new Views.AddTeacherView();
            addTeacherView.ShowDialog();
            Teachers = new ObservableCollection<Teacher>(new TeacherRepository().GetAllTeachers());
        }
        private bool CanExecuteEditOrDeleteTeacher(object obj)
        {
            return SelectedTeacher != null;
        }
        private void ExecuteEditTeacherCommand(object obj)
        {
            if (SelectedTeacher == null) return;
            var editTeacherView = new Views.EditTeacherView(SelectedTeacher);
            editTeacherView.ShowDialog();
            Teachers = new ObservableCollection<Teacher>(new TeacherRepository().GetAllTeachers());
        }
        private void ExecuteDeleteTeacherCommand(object obj)
        {
            if (SelectedTeacher == null) return;
            if (System.Windows.MessageBox.Show($"Are you sure you want to delete teacher {SelectedTeacher.User.FullName}?", "Confirm Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes)
            {
                new TeacherRepository().DeleteTeacher(SelectedTeacher.TeacherId);
                Teachers = new ObservableCollection<Teacher>(new TeacherRepository().GetAllTeachers());
            }
        }
    }
} 