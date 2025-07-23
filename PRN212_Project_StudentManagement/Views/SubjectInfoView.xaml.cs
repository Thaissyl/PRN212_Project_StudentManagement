using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class SubjectInfoView : Window
    {
        public SubjectInfoView()
        {
            InitializeComponent();
            DataContext = new SubjectInfoViewModel();
        }
    }
} 