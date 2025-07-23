using PRN212_Project_StudentManagement.Data.Repositories;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class EditSubjectViewModel : ViewModelBase
    {
        private int _subjectId;
        private string _subjectName;
        public string SubjectName
        {
            get => _subjectName;
            set { _subjectName = value; OnPropertyChanged(nameof(SubjectName)); }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public EditSubjectViewModel(int subjectId, string subjectName)
        {
            _subjectId = subjectId;
            SubjectName = subjectName;
            SaveCommand = new ViewModelCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }
        private bool CanExecuteSaveCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(SubjectName);
        }
        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                new SubjectRepository().UpdateSubject(_subjectId, SubjectName);
                MessageBox.Show("Subject updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                (obj as Window)?.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error updating subject: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteCancelCommand(object obj)
        {
            (obj as Window)?.Close();
        }
    }
} 