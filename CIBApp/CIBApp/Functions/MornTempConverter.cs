﻿/// <summary>
/// 
/// </summary>

namespace CIBApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Xamarin.Forms;

    public class MornTempConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double)value;
            var round = Math.Ceiling(doubleValue);
            var converted = $"but the actual temperature is {round}°C.";
            return converted;
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
