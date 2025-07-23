using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class TeacherInfoView : Window
    {
        public TeacherInfoView()
        {
            InitializeComponent();
            DataContext = new TeacherInfoViewModel();
        }
    }
} 