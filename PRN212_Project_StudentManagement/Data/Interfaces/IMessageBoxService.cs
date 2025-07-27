using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project_StudentManagement.Data.Interfaces
{
    public interface IMessageBoxService
    {
        bool ConfirmDelete(string message);
    }
}
