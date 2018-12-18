using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApplication1.elements.converter
{
    internal class MinusAbsolutValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().IsEquivalentTo(targetType)) {
                return int.Parse(value.ToString()) - int.Parse(parameter.ToString());
            }
            else { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType().IsEquivalentTo(targetType))
            {
                return int.Parse(value.ToString()) + int.Parse(parameter.ToString());
            }
            else { return value; }
        }
    }
}
