using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace ToolKit
{
	public class ToolKitDataContext : INotifyPropertyChanged
	{
		//InotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ToolKitDataContext()
		{
			_tileRessource = new Dictionary<string, BitmapImage>();
		}

		public Level Level { get; set; }
		public Dictionary<string, BitmapImage> TileRessource { get => _tileRessource; set => _tileRessource = value; }

		private  Dictionary<string, BitmapImage> _tileRessource = null;

		internal void LoadXMLTemplate(string XMLFileName)
		{
			Level = XMLHelper.ReadXMLToObject<Level>(XMLFileName);
		}

		internal void LoadTileImages(string contentPath)
		{
			var filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, contentPath));

			foreach (var filePath in filePaths)
			{
				_tileRessource.Add(Path.GetFileNameWithoutExtension(filePath), new BitmapImage(new Uri(filePath)));
			}
		}

	}
}
