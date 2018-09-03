using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class ExitVictoryCondition : IVictoryCondition
	{
		private string _assetName;
		private string _nextLevel;

		public string ExitAssetName
		{
			get { return _assetName; }
			set { _assetName = value; }
		}

		public string NextLevel { get => _nextLevel; set => _nextLevel = value; }

		//Check if player position is equal to exit asset
		public bool IsConditionComplete(Level level)
		{
			//Get floor layer
			level.Layers.TryGetValue(0f, out SpriteBase[,] floor);

			for (int i = 0; i < level.Rows; i++)
			{
				for (int j = 0; j < level.Columns; j++)
				{
					if (floor[i, j] != null && floor[i, j].Position == level.Player.Position)
					{
						if (floor[i, j].Texture.Name == this.ExitAssetName)
							return true;
					}
				}
			}

			return false;

		}
	}
}
