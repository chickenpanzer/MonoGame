using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit
{
	internal class LevelGenerator
	{
		private int _rows;
		private int _columns;

		public LevelGenerator(int numberOfRows, int numberOfColumns)
		{

			_rows = numberOfRows;
			_columns = numberOfColumns;

		}

		internal Level GenerateLevel(string levelName, string defaultAsset = null)
		{
			Level level = new Level();

			level.columns = _columns.ToString();
			level.rows = _rows.ToString();

			level.name = levelName;
			level.Content = new LevelContent();
			level.Tiles = new LevelTilesTile[_rows * _columns];

			GenerateTiles(level, defaultAsset);

			return level;
		}

		private void GenerateTiles(Level level, string defaultAsset)
		{
			int k = 0;

			for (int i = 0; i < _rows; i++)
			{
				for (int j = 0; j < _columns; j++)
				{
					var tile = new LevelTilesTile()
					{
						posX = j.ToString(),
						posY = i.ToString(),
						isWalkable = "True"
					};

					var layerZero = new LevelTilesTileLayer()
					{
						depth = "0",
						assetName = defaultAsset
					};

					var dummyActor = new LevelTilesTileActor();

					tile.Layer = new LevelTilesTileLayer[1];
					tile.Layer[0] = layerZero;

					tile.Actor = new LevelTilesTileActor[1];
					tile.Actor[0] = dummyActor;

					level.Tiles[k] = tile;
					k++;
				}
			}
		}
	}
}
