using PRN212_Project_StudentManagement.Data.Interfaces;
using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class MainClassViewModel : ViewModelBase
    {
        private readonly IClassRepository _classRepository;
        private ObservableCollection<Class> _classes;
        private Class _selectedClass;

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

        public MainClassViewModel()
        {
            _classRepository = new ClassRepository();
            AddClassCommand = new ViewModelCommand(ExecuteAddClassCommand, CanExecuteAddClassCommand);
            UpdateClassCommand = new ViewModelCommand(ExecuteUpdateClassCommand, CanExecuteUpdateClassCommand);
            DeleteClassCommand = new ViewModelCommand(ExecuteDeleteClassCommand, CanExecuteDeleteClassCommand);
            LoadClasses();
        }

        private void LoadClasses()
        {
            Classes = new ObservableCollection<Class>(_classRepository.GetAll());
        }

        private bool CanExecuteAddClassCommand(object obj)
        {
            return SelectedClass != null && !string.IsNullOrWhiteSpace(SelectedClass.ClassName);
        }

        private void ExecuteAddClassCommand(object obj)
        {
            _classRepository.AddClass(new Class { ClassName = SelectedClass.ClassName });
            LoadClasses();
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
    }
}
