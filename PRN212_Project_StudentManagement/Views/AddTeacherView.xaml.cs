using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class AddTeacherView : Window
    {
        public AddTeacherView()
        {
            InitializeComponent();
            DataContext = new AddTeacherViewModel();
        }
    }
} 