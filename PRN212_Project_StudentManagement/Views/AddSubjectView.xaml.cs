using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class AddSubjectView : Window
    {
        public AddSubjectView()
        {
            InitializeComponent();
            DataContext = new AddSubjectViewModel();
        }
    }
} 