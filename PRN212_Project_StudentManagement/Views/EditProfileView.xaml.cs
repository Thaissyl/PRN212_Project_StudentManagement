using PRN212_Project_StudentManagement.Data.Repositories;
using PRN212_Project_StudentManagement.Models;
using PRN212_Project_StudentManagement.ViewModels;
using System.Windows;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class EditProfileView : Window
    {
        public EditProfileView(User user)
        {
            InitializeComponent();
            DataContext = new EditProfileViewModel(new UserRepository(), user, this);
        }
    }
}
