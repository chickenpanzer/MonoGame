﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Penumbra;

namespace MonoGame.Core
{
	public class Level : Component
	{
		#region Private
		//Level size
		private int _rows;
		private int _columns;
		private Game _game;
		private PenumbraComponent _penumbra;
		private List<SpriteBase> _actors = new List<SpriteBase>();
		private Player _player;

		private Song _bgMusic = null;

		//Sounds
		private Dictionary<string, SoundEffect> _soundEffects = null;

		//Textures
		private Dictionary<string, Texture2D> _textures = null;

		//Layers
		private Dictionary<float, SpriteBase[,]> _layers = null;
		#endregion

		#region Public Properties
		public SpriteBase Player { get; set; }
		#endregion

		public Level(Game game, PenumbraComponent penumbra)
		{
			_textures = new Dictionary<string, Texture2D>();
			_soundEffects = new Dictionary<string, SoundEffect>();
			_layers = new Dictionary<float, SpriteBase[,]>();
			_game = game;
			_penumbra = penumbra;
		}

		public void Begin(Player player)
		{
			_player = player;
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.4f;
			MediaPlayer.Play(_bgMusic);

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

			InitPenumbraHulls(_penumbra, _layers);
		}

		private void InitPenumbraHulls(PenumbraComponent penumbra, Dictionary<float, SpriteBase[,]> layers)
		{
			//Get floor layer
			_layers.TryGetValue(0f, out SpriteBase[,] floor);

			for (int i = 0; i < _rows; i++)
			{
				for (int j = 0; j < _columns; j++)
				{
					if (floor[i, j] != null && ((Floor)floor[i, j]).IsWalkable == false)
						AddHull(penumbra, floor[i, j].Position);
				}
			}


		}

		private void AddHull(PenumbraComponent penumbra, Vector2 position)
		{
			float x = position.X;
			float y = position.Y;

			Hull newHull = new Hull(position, new Vector2(x + 32, y), new Vector2(x + 32, y + 32), new Vector2(x, y + 32));
			penumbra.Hulls.Add(newHull);
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
				InitTileActors(tile, _penumbra);
			}
		}

		private void InitTileActors(XElement tile, PenumbraComponent penumbra)
		{
			Assembly assem = Assembly.GetExecutingAssembly();
			string assemblyName = assem.GetName().Name;
			var actors = tile.Descendants("Actor");

			foreach (var actor in actors)
			{
				//CreateInstance class
				var hndl = Activator.CreateInstance(assemblyName, assemblyName + "." + actor.Attribute("class").Value);
				var instance = hndl.Unwrap();

				//Get texture
				_textures.TryGetValue(actor.Attribute("assetName").Value, out Texture2D texture);

				int.TryParse(tile.Attribute("posX").Value, out int posX);
				int.TryParse(tile.Attribute("posY").Value, out int posY);


				((SpriteBase)instance).Texture = texture;
				((SpriteBase)instance).Position = new Vector2(posX * 32, posY * 32);
				((SpriteBase)instance).Layer = 0.6f;

				//Light emitting actor ?
				try
				{
					string lightScale = actor.Attribute("lightScale").Value;

					if (!string.IsNullOrEmpty(lightScale))
					{
						float.TryParse(lightScale, out float scale);

						float x = ((SpriteBase)instance).Position.X + 16;
						float y = ((SpriteBase)instance).Position.Y + 16;


						penumbra.Lights.Add(new PointLight
						{
							Scale = new Vector2(scale), // Range of the light source (how far the light will travel)
							ShadowType = ShadowType.Illuminated, // Will not lit hulls themselves
							Color = Color.MonoGameOrange,
							Position = new Vector2(x,y)
						});
					}
				}
				catch (Exception)
				{
				}


				//Actor Pickup logic
				if (instance.GetType() == typeof(Pickup))
				{
					InitActorPickup(actor, instance);
				}

				//Actor Monster logic
				if (instance.GetType() == typeof(Monster))
				{
					InitActorMonster(actor, instance);
				}

				//Actor Mover Class if any
				var moverClass = actor.Descendants("Mover").FirstOrDefault();
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

				//Add sprite in level sprites list
				_actors.Add((SpriteBase)instance);
			}
		}

		private static void InitActorMonster(XElement actor, object instance)
		{
			var monster = ((Monster)instance);

			monster.Layer = 0.5f;

			int healthValue = 0;
			int scoreValue = 0;
			int attackValue = 0;
			int defenseValue = 0;

			if (actor.Attribute("healthValue") != null)
				int.TryParse(actor.Attribute("healthValue").Value, out healthValue);

			if (actor.Attribute("scoreValue") != null)
				int.TryParse(actor.Attribute("scoreValue").Value, out scoreValue);

			if (actor.Attribute("attackValue") != null)
				int.TryParse(actor.Attribute("attackValue").Value, out attackValue);

			if (actor.Attribute("defenseValue") != null)
				int.TryParse(actor.Attribute("defenseValue").Value, out defenseValue);

			monster.Health = healthValue;
			monster.Score = scoreValue;
			monster.Attack = attackValue;
			monster.Defense = defenseValue;
		}

		private static void InitActorPickup(XElement actor, object instance)
		{
			var pickup = ((Pickup)instance);

			pickup.Layer = 0.4f;

			int healthValue = 0;
			int scoreValue = 0;
			int attackValue = 0;
			int defenseValue = 0;

			if (actor.Attribute("healthValue") != null)
				int.TryParse(actor.Attribute("healthValue").Value, out healthValue);

			if (actor.Attribute("scoreValue") != null)
				int.TryParse(actor.Attribute("scoreValue").Value, out scoreValue);

			if (actor.Attribute("attackValue") != null)
				int.TryParse(actor.Attribute("attackValue").Value, out attackValue);

			if (actor.Attribute("defenseValue") != null)
				int.TryParse(actor.Attribute("defenseValue").Value, out defenseValue);

			pickup.HealthValue = healthValue;
			pickup.ScoreValue = scoreValue;
			pickup.AttackValue = attackValue;
			pickup.DefenseValue = defenseValue;
			pickup.PickupSound = actor.Attribute("pickupSound").Value;
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

			//Sounds
			var soundInfo = root.Descendants("SoundRessource");

			foreach (var si in soundInfo)
			{
				string key = si.Attribute("key").Value;
				_soundEffects.Add(key, _game.Content.Load<SoundEffect>(key));
			}

			//BackgroundMusic
			var bgInfo = root.Descendants("BackgroundMusic").FirstOrDefault();

			if (bgInfo != null)
			{
				string key = bgInfo.Attribute("key").Value;
				_bgMusic = _game.Content.Load<Song>(key);
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

			//Draw actors
			foreach (var actor in _actors)
			{
				actor.Draw(gameTime, spriteBatch);
			}
		}

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//Update level layers
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

			//Update actors
			UpdateActors(gameTime, sprites);

			//Check for collisions between player and actors
			foreach (var actor in _actors)
			{
				if (_player.Position.Equals(actor.Position))
				{
					if (actor.GetType() == typeof(Pickup))
					{
						var item = ((Pickup)actor);

						_player.Health += item.HealthValue;
						_player.Score += item.ScoreValue;
						_player.Attack += item.AttackValue;
						_player.Defense += item.DefenseValue;

						Debug.Print(_player.ToString());

						//play pickup sound
						_soundEffects.TryGetValue(item.PickupSound, out SoundEffect sound);
						sound.CreateInstance().Play();
						actor.IsAlive = false;

						RemoveLightAtPosition(actor.Position);
					}

					if (actor.GetType() == typeof(Monster))
					{
						var monster = ((Monster)actor);

						_player.Health -= Math.Max(monster.Attack - _player.Defense,0);
						_player.Score += monster.Score;

						//wearing armor
						_player.Defense -= Math.Max(monster.Attack / 3, 0);

						actor.IsAlive = false;
					}
				}
			}
			
			//Remove
			_actors.RemoveAll(m => m.IsAlive == false);
		}

		/// <summary>
		/// Remove light if any after pickup object
		/// </summary>
		/// <param name="position"></param>
		private void RemoveLightAtPosition(Vector2 position)
		{
			foreach (var light in _penumbra.Lights.ToArray())
			{

				float x = position.X + 16;
				float y = position.Y + 16;

				Vector2 lightPosition = new Vector2(x, y);


				if (light.Position.Equals(lightPosition))
				{
					_penumbra.Lights.Remove(light);
				}

			}
		}

		private void UpdateActors(GameTime gameTime, List<SpriteBase> sprites)
		{
			foreach (var actor in _actors)
			{
				Vector2 oldPosition = actor.Position;
				actor.Update(gameTime, sprites);

				//block illegal movement
				int X = (int)actor.Position.X / 32;
				int Y = (int)actor.Position.Y / 32;

				SpriteBase[,] floorTiles = null;
				_layers.TryGetValue(0f, out floorTiles);

				try
				{
					if (((Floor)floorTiles[X, Y]).IsWalkable == false)
						actor.Position = oldPosition;

					if (!actor.Position.Equals(oldPosition))
						Debug.Print(actor.ToString());

				}

				catch (Exception)
				{
				}
			}
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

				if (!_player.Position.Equals(oldPosition))
				{
					_player.Health -= 1;
					Debug.Print(_player.ToString());

				}
			}
			catch (Exception)
			{
				//Out of bounds : stay put !
				_player.Position = oldPosition;
			}

		}
	}
}