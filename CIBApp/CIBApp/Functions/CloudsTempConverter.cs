/// <summary>
/// 
/// </summary>
namespace CIBApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Xamarin.Forms;

    public class CloudsTempConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = (int)value;
            //var round = Math.Ceiling(doubleValue);
            var converted = $"Cloud lurks at the percentage of {intValue}%,";
            return converted;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
