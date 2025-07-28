using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class TeacherInfoViewModel : ViewModelBase
    {
        private readonly ITeacherRepository _repository;
        private readonly IMessageBoxService _messageBoxService;

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
        private bool _isTest;
        public TeacherInfoViewModel(ITeacherRepository repository, IMessageBoxService messageBoxService, bool isTest = false)
        {
            _repository = repository;
            _messageBoxService = messageBoxService;
            _isTest = isTest;
            Teachers = new ObservableCollection<Teacher>(_repository.GetAllTeachers());

            AddTeacherCommand = new ViewModelCommand(ExecuteAddTeacherCommand);
            EditTeacherCommand = new ViewModelCommand(ExecuteEditTeacherCommand, CanExecuteEditOrDeleteTeacher);
            DeleteTeacherCommand = new ViewModelCommand(ExecuteDeleteTeacherCommand, CanExecuteEditOrDeleteTeacher);
        }

        public TeacherInfoViewModel(ITeacherRepository repository) : this(repository, new Data.Services.MessageBoxService())
        {
        }

        public TeacherInfoViewModel()
        {
        }

        private void LoadTeachers()
        {
            Teachers = new ObservableCollection<Teacher>(_repository.GetAllTeachers());
        }
        private void ExecuteAddTeacherCommand(object obj)
        {
            if (!_isTest)
            {
                var addTeacherView = new Views.AddTeacherView();
                addTeacherView.ShowDialog();
            }

            LoadTeachers(); // vẫn chạy bình thường
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
            LoadTeachers();
        }
        private void ExecuteDeleteTeacherCommand(object obj)
        {
            if (SelectedTeacher == null) return;

            bool confirm = _messageBoxService.ConfirmDelete(
                $"Are you sure you want to delete teacher {SelectedTeacher.User.FullName}?"
            );

            if (confirm)
            {
                _repository.DeleteTeacher(SelectedTeacher.TeacherId);
                LoadTeachers();
            }

        }
    }
}
