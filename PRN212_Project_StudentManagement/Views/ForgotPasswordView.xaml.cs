using System.Windows;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    public partial class ForgotPasswordView : Window
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
            DataContext = new ForgotPasswordViewModel(new Data.Repositories.UserRepository(), this);
        }

    }
}
