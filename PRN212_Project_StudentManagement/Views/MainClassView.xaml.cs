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
using PRN212_Project_StudentManagement.ViewModels;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRN212_Project_StudentManagement.Views
{
    /// <summary>
    /// Interaction logic for MainClassView.xaml
    /// </summary>
    public partial class MainClassView : Window
    {
        public MainClassView()
        {
            InitializeComponent();
            DataContext = new MainClassViewModel();
        }
    }
}
