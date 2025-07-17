using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class AddClassViewModel : ViewModelBase
    {
        private readonly IClassRepository _classRepository;
        private readonly ITeacherRepository _teacherRepository;
        private string _className;
        private int _selectedTeacherId;
        private string _academicYear;
        private ObservableCollection<Teacher> _teachers;
        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }
        public int SelectedTeacherId
        {
            get { return _selectedTeacherId; }
            set
            {
                _selectedTeacherId = value;
                OnPropertyChanged(nameof(SelectedTeacherId));
            }
        }

        public string AcademicYear
        {
            get { return _academicYear; }
            set
            {
                _academicYear = value;
                OnPropertyChanged(nameof(AcademicYear));
            }
        }

        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set
            {
                _teachers = value;
                OnPropertyChanged(nameof(Teachers));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddClassViewModel()
        {
            _classRepository = new ClassRepository();
            _teacherRepository = new TeacherRepository();
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            Teachers = new ObservableCollection<Teacher>(_teacherRepository.GetAllTeachers());
        }

        private bool CanExecuteSaveCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(ClassName) && SelectedTeacherId > 0 && !string.IsNullOrWhiteSpace(AcademicYear);
        }

        private void ExecuteSaveCommand(object obj)
        {
            var newClass = new Class
            {
                ClassName = this.ClassName,
                TeacherId = this.SelectedTeacherId,
                AcademicYear = this.AcademicYear
            };
            _classRepository.AddClass(newClass);
            (obj as Window)?.Close();
        }

        private void ExecuteCancelCommand(object obj)
        {
            (obj as Window)?.Close();
        }
    }
}
