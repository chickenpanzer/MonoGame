using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Level : Component
	{
		//Level size
		private int _rows;
		private int _columns;
		private Game _game;

		//Textures
		private Dictionary<string, Texture2D> _textures = null;

		//Layers
		private Dictionary<float, SpriteBase[,]> _layers = null;


		public Level(Game game)
		{
			_textures = new Dictionary<string, Texture2D>();
			_layers = new Dictionary<float, SpriteBase[,]>();
			_game = game;
		}

		public void LoadLevel(string xmlFileName)
		{
			//Retrieve level name and size
			XDocument doc = null;
			using (FileStream streamReader = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFileName), FileMode.Open))
			{
				doc = XDocument.Load(streamReader);

				_rows = int.Parse(doc.Root.Attribute("rows").Value);
				_columns = int.Parse(doc.Root.Attribute("columns").Value);

			}

			//Sizing level 
			_layers.Add(0f, new SpriteBase[_rows, _columns]);

			//Loading content
			LoadContent(_game, doc);
			InitLayers(doc);
		}

		private void InitLayers(XDocument doc)
		{
			//Filling tiles
			var tiles = doc.Descendants().Where(e => e.Name == "Tile");

			foreach (var tile in tiles)
			{
				int posX = int.Parse(tile.Attribute("posX").Value);
				int posY = int.Parse(tile.Attribute("posY").Value);
				string assetName = String.Empty;
				float depth = 0f;

				//layers
				var layers = tile.Descendants("Layer");
				foreach (var layer in layers)
				{
					assetName = layer.Attribute("assetName").Value;
					depth = float.Parse(layer.Attribute("depth").Value);

					//Get texture
					Texture2D texture = null;
					_textures.TryGetValue(assetName, out texture);

					//Adding layer
					if (!_layers.ContainsKey(depth))
					{
						_layers.Add(depth, new SpriteBase[_rows, _columns]);
					}

					SpriteBase[,] grid = null;

					_layers.TryGetValue(depth, out grid);
					grid[posX, posY] = new SpriteBase(texture, new Vector2(posX * 32, posY * 32), null, depth);
				}
			}
		}

		/// <summary>
		/// Load Textures and stuff
		/// </summary>
		/// <param name="game"></param>
		/// <param name="doc"></param>
		private void LoadContent(Game game, XDocument doc)
		{
			var root = doc.Root;

			//Textures
			var textureInfo = root.Descendants("TextureRessource");

			foreach (var ti in textureInfo)
			{
				string key = ti.Attribute("key").Value;
				_textures.Add(key, _game.Content.Load<Texture2D>(key));
			}
		}

		/// <summary>
		/// Draw level tiles.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

			foreach (var layer in _layers)
			{
				for (int i = 0; i < _rows; i++)
				{
					for (int j = 0; j < _columns; j++)
					{
						if (layer.Value[i, j] != null)
							layer.Value[i, j].Draw(gameTime, spriteBatch);
					}
				}
			}
		}

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//No update logic here
		}
	}
}
