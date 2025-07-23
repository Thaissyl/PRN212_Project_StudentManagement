using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class EditSubjectView : Window
    {
        public EditSubjectView(int subjectId, string subjectName)
        {
            InitializeComponent();
            DataContext = new EditSubjectViewModel(subjectId, subjectName);
        }
    }
} 