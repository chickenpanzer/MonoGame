using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ToolKit
{
	public class TileRessource
	{

		public Dictionary<string, BitmapImage> BitmapDictionary { get; set; }

		public TileRessource()
		{

			BitmapDictionary = new Dictionary<string, BitmapImage>();

			var filePaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Floor"));

			foreach (var filePath in filePaths)
			{
				BitmapDictionary.Add(Path.GetFileNameWithoutExtension(filePath), new BitmapImage(new Uri(filePath)));
			}
		}

	}
}
