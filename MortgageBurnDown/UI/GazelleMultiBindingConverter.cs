using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MortgageBurnDown
{
    public class GazelleMultiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal value;
            if (values.Length == 3 && values[1] != null && decimal.TryParse(values[2] as string, out value))
            {
                string command = values[0] as string;
                DateTime date = (DateTime)values[1];

                return new Tuple<string, DateTime, decimal>(command, date, value);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
