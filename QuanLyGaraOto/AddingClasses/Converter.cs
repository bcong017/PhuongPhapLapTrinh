using QuanLyGaraOto.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyGaraOto.AddingClasses
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal fraction;
            if (string.IsNullOrEmpty(value.ToString()))
                fraction = 0;
            fraction = decimal.Parse(value.ToString());
            return fraction.ToString("P2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Trim any trailing percentage symbol that the user MAY have included
            var valueWithoutPercentage = value.ToString().TrimEnd(' ', '%');
            return decimal.Parse(valueWithoutPercentage) / 100;
        }
    }

    public class YearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Month selectedItem = value as Month;
            if (selectedItem == null)
                return null;
            return selectedItem.Number;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
