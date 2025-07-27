using PRN212_Project_StudentManagement.ViewModels;
using System.Windows;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class StudentMarkView : Window
    {
        public StudentMarkView(StudentMarkViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 