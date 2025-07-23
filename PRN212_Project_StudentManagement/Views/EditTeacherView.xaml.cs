using System.Windows;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class EditTeacherView : Window
    {
        public EditTeacherView(Teacher teacher)
        {
            InitializeComponent();
            DataContext = new EditTeacherViewModel(teacher);
        }
    }
} 