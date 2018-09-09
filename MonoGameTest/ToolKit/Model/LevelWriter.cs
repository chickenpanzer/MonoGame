using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit
{
	internal class LevelWriter
	{

		public Level AddRessourcesToLevel(Level level)
		{
			//Reset texture ressources
			level.Content.TextureRessource = null;

			//Tile ressources
			level = AddTileAssets(level);

			//Actor ressources
			level = AddActorAssets(level);

			return level;
		}

		private Level AddActorAssets(Level level)
		{
			Dictionary<string, string> assetList = new Dictionary<string, string>();

			//Gather asset info
			foreach (var tile in level.Tiles)
			{
				//Get Actor
				if (tile.Actor != null)
				{

					var actor = tile.Actor[0];

					if (!string.IsNullOrEmpty(actor.assetName))
					{
						try
						{
							assetList.Add(actor.assetName, actor.assetName);
						}
						catch (Exception)
						{
						}
					}
				}
			}

			//Write asset info
			foreach (var asset in assetList.Keys)
			{
				var textureRessource = new LevelContentTextureRessource() { key = asset };
				level.Content.TextureRessource = level.Content.TextureRessource.Redim(true);
				level.Content.TextureRessource[level.Content.TextureRessource.Length - 1] = textureRessource;
			}

			return level;
		}

		private Level AddTileAssets(Level level)
		{
			Dictionary<string, string> assetList = new Dictionary<string, string>();

			//Gather asset info
			foreach (var tile in level.Tiles)
			{
				//Get layer0
				var layer = tile.Layer[0];

				try
				{
					assetList.Add(layer.assetName, layer.assetName); 
				}
				catch (Exception)
				{
				}
			}

			//Write asset info
			foreach (var asset in assetList.Keys)
			{
				var textureRessource = new LevelContentTextureRessource() { key = asset };
				level.Content.TextureRessource = level.Content.TextureRessource.Redim(true);
				level.Content.TextureRessource[level.Content.TextureRessource.Length - 1] = textureRessource;
			}

			return level;

		}
	}
}
