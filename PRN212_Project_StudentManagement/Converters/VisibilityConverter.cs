using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PRN212_Project_StudentManagement.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public static VisibilityConverter Instance { get; } = new VisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 