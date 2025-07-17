using PRN212_Project_StudentManagement.ViewModels;
using System.Windows;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class AddClassView : Window
    {
        public AddClassView()
        {
            InitializeComponent();
            DataContext = new AddClassViewModel();
        }
    }
}
