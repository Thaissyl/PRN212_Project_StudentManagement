using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project_StudentManagement.Converters
{
    public class MarkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the double to a string for the TextBox.
            // Use InvariantCulture to ensure '.' is used as the decimal separator.
            if (value is double d)
            {
                return d.ToString(CultureInfo.InvariantCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // When converting back, try to parse the string to a double.
            // Use InvariantCulture to handle '.' as the decimal separator.
            if (value is string str && double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            // If parsing fails, return DoNothing to keep the old value.
            return Binding.DoNothing;
        }
    }
}
