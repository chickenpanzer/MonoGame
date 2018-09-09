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
			//Reset texture ressources and sound ressources
			level.Content.TextureRessource = null;
			level.Content.SoundRessource = null;

			//Tile ressources
			level = AddTileAssets(level);

			//Actor ressources
			level = AddActorAssets(level);

			return level;
		}

		private Level AddActorAssets(Level level)
		{
			Dictionary<string, string> assetList = new Dictionary<string, string>();
			Dictionary<string, string> soundList = new Dictionary<string, string>();

			//Gather asset info
			foreach (var tile in level.Tiles)
			{
				//Get Actor
				if (tile.Actor != null)
				{

					var actor = tile.Actor[0];

					if (!string.IsNullOrEmpty(actor.assetName) && !assetList.ContainsKey(actor.assetName))
						assetList.Add(actor.assetName, actor.assetName);

					if(!string.IsNullOrEmpty(actor.pickupSound) && !soundList.ContainsKey(actor.pickupSound))
						soundList.Add(actor.pickupSound, actor.pickupSound);
				}
			}

			//Write TextureRessource
			foreach (var asset in assetList.Keys)
			{
				var textureRessource = new LevelContentTextureRessource() { key = asset };
				level.Content.TextureRessource = level.Content.TextureRessource.Redim(true);
				level.Content.TextureRessource[level.Content.TextureRessource.Length - 1] = textureRessource;
			}

			//Write SoundRessource
			foreach (var sound in soundList.Keys)
			{
				var soundRessource = new LevelContentSoundRessource() { key = sound };
				level.Content.SoundRessource = level.Content.SoundRessource.Redim(true);
				level.Content.SoundRessource[level.Content.SoundRessource.Length - 1] = soundRessource;
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

				if (!string.IsNullOrEmpty(layer.assetName) && !assetList.ContainsKey(layer.assetName))
					assetList.Add(layer.assetName, layer.assetName);
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
