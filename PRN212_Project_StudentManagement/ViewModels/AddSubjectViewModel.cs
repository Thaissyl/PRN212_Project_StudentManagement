using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using System.Windows;
using System.Windows.Input;

namespace PRN212_Project_StudentManagement.ViewModels
{
    public class AddSubjectViewModel : ViewModelBase
    {
        private string _subjectName;
        public string SubjectName
        {
            get => _subjectName;
            set { _subjectName = value; OnPropertyChanged(nameof(SubjectName)); }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public AddSubjectViewModel()
        {
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
                new SubjectRepository().AddSubject(new Subject { SubjectName = this.SubjectName });
                MessageBox.Show("Subject added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                (obj as Window)?.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error adding subject: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteCancelCommand(object obj)
        {
            (obj as Window)?.Close();
        }
    }
} 