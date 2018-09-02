using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
		#region Private
		//Level size
		private int _rows;
		private int _columns;
		private Game _game;
		private List<SpriteBase> _mobs = new List<SpriteBase>();
		private SpriteBase _player;
		

		//Textures
		private Dictionary<string, Texture2D> _textures = null;

		//Layers
		private Dictionary<float, SpriteBase[,]> _layers = null;
		#endregion

		#region Public Properties
		public SpriteBase Player { get; set; }
		#endregion

		public Level(Game game)
		{
			_textures = new Dictionary<string, Texture2D>();
			_layers = new Dictionary<float, SpriteBase[,]>();
			_game = game;
		}

		public void Begin(SpriteBase player)
		{
			_player = player;
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

				//Tile layers
				InitTileLayers(tile, posX, posY, ref assetName, ref depth);

				//Mobs
				InitTileMobs(tile);
			}
		}

		private void InitTileMobs(XElement tile)
		{
			Assembly assem = Assembly.GetExecutingAssembly();
			string assemblyName = assem.GetName().Name;
			var mobs = tile.Descendants("Mob");

			foreach (var mob in mobs)
			{
				//Mob class
				var hndl = Activator.CreateInstance(assemblyName, assemblyName + "." + mob.Attribute("class").Value);
				var instance = hndl.Unwrap();

				//Get texture
				_textures.TryGetValue(mob.Attribute("assetName").Value, out Texture2D texture);

				int.TryParse(tile.Attribute("posX").Value, out int posX);
				int.TryParse(tile.Attribute("posY").Value, out int posY);


				((SpriteBase)instance).Texture = texture;
				((SpriteBase)instance).Position = new Vector2(posX * 32, posY * 32);
				((SpriteBase)instance).Layer = 1f;

				//Mover Class if any
				var moverClass = mob.Descendants("Mover").FirstOrDefault();
				if (moverClass != null)
				{
					hndl = Activator.CreateInstance(assemblyName, assemblyName + "." + moverClass.Attribute("class").Value);
					var mover = hndl.Unwrap();

					float.TryParse(moverClass.Attribute("moveSpeed").Value, out float speed);
					int.TryParse(moverClass.Attribute("interval").Value, out int interval);

					((IMoveable)mover).MoveSpeed = speed;
					((IMoveable)mover).Interval = interval;

					//Inject mover
					((SpriteBase)instance).Mover = (IMoveable)mover;

				}

				//Add sprite in sprites
				_mobs.Add((SpriteBase)instance);
			}
		}

		private void InitTileLayers(XElement tile, int posX, int posY, ref string assetName, ref float depth)
		{
			bool.TryParse(tile.Attribute("isWalkable").Value, out bool isWalkable);

			//layers
			var layers = tile.Descendants("Layer");
			foreach (var layer in layers)
			{
				assetName = layer.Attribute("assetName").Value;
				depth = float.Parse(layer.Attribute("depth").Value);

				//Get texture
				_textures.TryGetValue(assetName, out Texture2D texture);

				//Adding layer
				if (!_layers.ContainsKey(depth))
				{
					_layers.Add(depth, new SpriteBase[_rows, _columns]);
				}

				_layers.TryGetValue(depth, out SpriteBase[,] grid);

				//Ground Zero !
				if (depth == 0f)
				{
					grid[posX, posY] = new Floor(texture, new Vector2(posX * 32, posY * 32), null, depth, isWalkable);
				}
				else
				{
					grid[posX, posY] = new SpriteBase(texture, new Vector2(posX * 32, posY * 32), null, 0f, Color.White, depth);
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

			//Draw level layers
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

			//Draw player
			_player.Draw(gameTime, spriteBatch);

			//Draw mobs
			foreach (var mob in _mobs)
			{
				mob.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//Update layers
			foreach (var layer in _layers)
			{
				for (int i = 0; i < _rows; i++)
				{
					for (int j = 0; j < _columns; j++)
					{
						if (layer.Value[i, j] != null)
							layer.Value[i, j].Update(gameTime, sprites);
					}
				}
			}

			//Update player
			UpdatePlayer(gameTime, sprites);

			//Update mobs
			foreach (var mob in _mobs)
			{
				mob.Update(gameTime, sprites);
			}

			//Check for collisions
			foreach (var mob in _mobs)
			{
				if (_player.Position.Equals(mob.Position))
					mob.IsAlive = false;
			}

			//Remove
			_mobs.RemoveAll(m => m.IsAlive == false);
		}

		private void UpdatePlayer(GameTime gameTime, List<SpriteBase> sprites)
		{
			Vector2 oldPosition = _player.Position;
			_player.Update(gameTime, sprites);

			//block illegal movement
			int X = (int)_player.Position.X / 32;
			int Y = (int)_player.Position.Y / 32;

			SpriteBase[,] floorTiles = null;
			_layers.TryGetValue(0f, out floorTiles);

			try
			{
				if (((Floor)floorTiles[X, Y]).IsWalkable == false)
					_player.Position = oldPosition;
			}
			catch (Exception)
			{
			}
		}
	}
}
