using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ToolKit
{
	public class AssetToBitmapConverter : IValueConverter
	{

		private Dictionary<string, BitmapImage> _bitmapDictionary { get; set; }

		public AssetToBitmapConverter()
		{

			_bitmapDictionary = new Dictionary<string, BitmapImage>();

			var filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Floor"));

			foreach (var filePath in filePaths)
			{
				_bitmapDictionary.Add(Path.GetFileNameWithoutExtension(filePath), new BitmapImage(new Uri(filePath)));
			}

			filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Items"));

			foreach (var filePath in filePaths)
			{
				_bitmapDictionary.Add(Path.GetFileNameWithoutExtension(filePath), new BitmapImage(new Uri(filePath)));
			}

			filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Monsters"));

			foreach (var filePath in filePaths)
			{
				_bitmapDictionary.Add(Path.GetFileNameWithoutExtension(filePath), new BitmapImage(new Uri(filePath)));
			}
		}


		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			BitmapImage result = null;

			if (_bitmapDictionary != null && !string.IsNullOrEmpty((string)value))
			{
				((Dictionary<string, BitmapImage>)_bitmapDictionary).TryGetValue((string)value, out result);
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
