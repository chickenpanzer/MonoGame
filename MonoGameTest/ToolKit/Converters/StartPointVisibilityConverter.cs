using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ToolKit
{
	public class StartPointVisibilityConverter : IMultiValueConverter
	{
		//Parameters must be actual PosX, PosY of tile, and Level start position
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{

			if (values[2] == null)
				return Visibility.Collapsed;

			var posX = values[0];
			var posY = values[1];
			var startPosition = ((string)values[2]).Split(',');

			if (startPosition.Length == 2)
			{
				if ((string)posX == startPosition[0] && (string)posY == startPosition[1])
				{
					return Visibility.Visible;
				}
			}
			else
			{
				return Visibility.Collapsed;
			}

			return Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
