using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project_StudentManagement.Data.Services
{
    internal class MessageBoxService
    {
        public bool ConfirmDelete(string message)
        {
            return System.Windows.MessageBox.Show(message, "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning) == System.Windows.MessageBoxResult.Yes;
        }
    }
}
