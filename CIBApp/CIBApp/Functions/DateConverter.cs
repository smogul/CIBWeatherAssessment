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

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fullTimeFormat = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var actualDateTime = (int)value;

            var converted = $"{fullTimeFormat.AddSeconds(actualDateTime).ToString()}";

            if (converted == null)
            {
                new NullReferenceException();
            }

            var dayOfTheWeek = (DateTime.Parse((converted.ToString()))).ToLongDateString();
           
            return dayOfTheWeek;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
