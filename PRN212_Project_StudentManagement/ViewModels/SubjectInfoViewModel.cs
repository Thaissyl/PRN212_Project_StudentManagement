using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class SubjectViewModel : ViewModelBase
    {
        public int Index { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }

    public class SubjectInfoViewModel : ViewModelBase
    {
        private ObservableCollection<SubjectViewModel> _subjects;
        public ObservableCollection<SubjectViewModel> Subjects
        {
            get => _subjects;
            set { _subjects = value; OnPropertyChanged(nameof(Subjects)); }
        }
        private SubjectViewModel _selectedSubject;
        public SubjectViewModel SelectedSubject
        {
            get => _selectedSubject;
            set { _selectedSubject = value; OnPropertyChanged(nameof(SelectedSubject)); }
        }
        public ICommand AddSubjectCommand { get; }
        public ICommand EditSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }

        public SubjectInfoViewModel()
        {
            LoadSubjects();
            AddSubjectCommand = new ViewModelCommand(ExecuteAddSubjectCommand);
            EditSubjectCommand = new ViewModelCommand(ExecuteEditSubjectCommand, CanExecuteEditOrDeleteSubject);
            DeleteSubjectCommand = new ViewModelCommand(ExecuteDeleteSubjectCommand, CanExecuteEditOrDeleteSubject);
        }

        private void LoadSubjects()
        {
            var repo = new SubjectRepository();
            var list = repo.GetAllSubjects().Select((s, i) => new SubjectViewModel { Index = i + 1, SubjectId = s.SubjectId, SubjectName = s.SubjectName }).ToList();
            Subjects = new ObservableCollection<SubjectViewModel>(list);
        }

        private void ExecuteAddSubjectCommand(object obj)
        {
            var addSubjectView = new AddSubjectView();
            addSubjectView.ShowDialog();
            LoadSubjects();
        }
        private bool CanExecuteEditOrDeleteSubject(object obj)
        {
            return SelectedSubject != null;
        }
        private void ExecuteEditSubjectCommand(object obj)
        {
            if (SelectedSubject == null) return;
            var editSubjectView = new Views.EditSubjectView(SelectedSubject.SubjectId, SelectedSubject.SubjectName);
            editSubjectView.ShowDialog();
            LoadSubjects();
        }
        private void ExecuteDeleteSubjectCommand(object obj)
        {
            if (SelectedSubject == null) return;
            if (MessageBox.Show($"Are you sure you want to delete subject '{SelectedSubject.SubjectName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                new SubjectRepository().DeleteSubject(SelectedSubject.SubjectId);
                LoadSubjects();
            }
        }
    }
} 