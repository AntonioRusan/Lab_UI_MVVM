using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace UIView
{
    public class BoundsTextBoxConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string result = values[0].ToString() + ";" + values[1].ToString();
                return result;
            }
            catch
            {
                MessageBox.Show("Неправильный ввод!");
                return values;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string[] splitValues = ((string)value).Split(';');
                double[] bounds = { System.Convert.ToDouble(splitValues[0]), System.Convert.ToDouble(splitValues[1]) };
                return new object[] { bounds[0], bounds[1] };
            }
            catch
            {
                return new object[] { 0, 1 };
            }
        }
    }
}
