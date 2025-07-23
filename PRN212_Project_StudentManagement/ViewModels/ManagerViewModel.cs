using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        private readonly IClassRepository _classRepository;
        private ObservableCollection<Class> _classes;
        private Class _selectedClass;
        private User _currentUser;

        public ObservableCollection<Class> Classes
        {
            get { return _classes; }
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        public Class SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        public ICommand AddClassCommand { get; }
        public ICommand UpdateClassCommand { get; }
        public ICommand DeleteClassCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddTeacherCommand { get; }
        public ICommand ShowTeacherInfoCommand { get; }
        public ICommand ShowSubjectInfoCommand { get; }

        public ManagerViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _classRepository = new ClassRepository();
            AddClassCommand = new ViewModelCommand(ExecuteAddClassCommand, CanExecuteAddClassCommand);
            UpdateClassCommand = new ViewModelCommand(ExecuteUpdateClassCommand, CanExecuteUpdateClassCommand);
            DeleteClassCommand = new ViewModelCommand(ExecuteDeleteClassCommand, CanExecuteDeleteClassCommand);
            RefreshCommand = new ViewModelCommand(ExecuteRefreshCommand);
            AddTeacherCommand = new ViewModelCommand(ExecuteAddTeacherCommand);
            ShowTeacherInfoCommand = new ViewModelCommand(ExecuteShowTeacherInfoCommand);
            ShowSubjectInfoCommand = new ViewModelCommand(ExecuteShowSubjectInfoCommand);
            LoadClasses();
        }

        private void LoadClasses()
        {
            Classes = new ObservableCollection<Class>(_classRepository.GetAllClasses());
        }

        private bool CanExecuteAddClassCommand(object obj)
        {
            return true;
        }

        private void ExecuteAddClassCommand(object obj)
        {
            var addClassView = new AddClassView();
            addClassView.ShowDialog();
            LoadClasses(); // Refresh the list after the dialog is closed
        }

        private bool CanExecuteUpdateClassCommand(object obj)
        {
            return SelectedClass != null && SelectedClass.ClassId > 0;
        }

        private void ExecuteUpdateClassCommand(object obj)
        {
            _classRepository.UpdateClass(SelectedClass);
            LoadClasses();
        }

        private bool CanExecuteDeleteClassCommand(object obj)
        {
            return SelectedClass != null && SelectedClass.ClassId > 0;
        }

        private void ExecuteDeleteClassCommand(object obj)
        {
            if (MessageBox.Show("Are you sure you want to delete this class?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _classRepository.DeleteClass(SelectedClass.ClassId);
                LoadClasses();
            }
        }

        private void ExecuteRefreshCommand(object obj)
        {
            // Clear the selected class to reset the details section
            SelectedClass = null;
            // Reload the classes list
            LoadClasses();
        }

        private void ExecuteAddTeacherCommand(object obj)
        {
            var addTeacherView = new Views.AddTeacherView();
            addTeacherView.ShowDialog();
            // Có thể load lại danh sách giáo viên nếu cần
        }

        private void ExecuteShowTeacherInfoCommand(object obj)
        {
            var teacherInfoView = new Views.TeacherInfoView();
            teacherInfoView.ShowDialog();
        }

        private void ExecuteShowSubjectInfoCommand(object obj)
        {
            var subjectInfoView = new Views.SubjectInfoView();
            subjectInfoView.ShowDialog();
        }
    }
}
