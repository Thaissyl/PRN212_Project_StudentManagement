using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PRN212_Project_StudentManagement.ViewModels;

namespace PRN212_Project_StudentManagement.Views
{
    /// <summary>
    /// Interaction logic for MainManagerView.xaml
    /// </summary>
    public partial class MainManagerView : Window
    {
        public MainManagerView()
        {
            InitializeComponent();
        }

        public MainManagerView(Models.User user)
        {
            InitializeComponent();
            DataContext = new ManagerViewModel(user);
        }
    }
}
