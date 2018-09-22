using System;
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
		private const int pix = 32;
		#region Private
		//Level size
		private int _rows;
		private int _columns;
		private string _nextLevel;

		private Game _game;
		private List<IVictoryCondition> _victoryConditions = new List<IVictoryCondition>();
		private bool _levelCompleted;

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
		private Vector2 _playerStartPosition;
		#endregion

		#region Public Properties
		public List<SpriteBase> Actors { get => _actors; }
		public List<IVictoryCondition> VictoryConditions { get => _victoryConditions; set => _victoryConditions = value; }
		public bool LevelCompleted { get => _levelCompleted; set => _levelCompleted = value; }
		public Dictionary<float, SpriteBase[,]> Layers { get => _layers; }
		public int Rows { get => _rows; }
		public int Columns { get => _columns; }
		public Player Player { get => _player; }
		public string NextLevel { get => _nextLevel; set => _nextLevel = value; }
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

			_player.Position = _playerStartPosition;

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


				string startPosition = doc.Root.Attribute("startPosition").Value;
				var pos = startPosition.Split(',');

				_playerStartPosition = new Vector2(float.Parse(pos[0]) * pix, float.Parse(pos[1]) * pix);

				if (doc.Root.Attribute("nextLevel") != null)
					_nextLevel = doc.Root.Attribute("nextLevel").Value;

			}

			//Sizing level 
			_layers.Add(0f, new SpriteBase[_rows, _columns]);

			//Loading victory conditions
			LoadVictoryConditions(doc);

			//Loading content
			LoadContent(_game, doc);

			//Initialize layers
			InitLayers(doc);

			InitPenumbraHulls(_penumbra, _layers);
		}

		private void LoadVictoryConditions(XDocument doc)
		{
			//Victory Conditions
			var vcInfo = doc.Root.Descendants("VictoryCondition");

			Assembly assem = Assembly.GetExecutingAssembly();
			string assemblyName = assem.GetName().Name;

			foreach (var vc in vcInfo)
			{
				//CreateInstance class
				var hndl = Activator.CreateInstance(assemblyName, assemblyName + "." + vc.Attribute("class").Value);
				var instance = hndl.Unwrap();

				//PickAllVictoryCondition
				if (instance.GetType() == typeof(PickAllVictoryCondition))
				{
					string assetName = vc.Attribute("assetName").Value;
					((PickAllVictoryCondition)instance).PickupAssetName = assetName;
					this.VictoryConditions.Add((PickAllVictoryCondition)instance);
				}

				//ExitVictoryCondition
				if (instance.GetType() == typeof(ExitVictoryCondition))
				{
					string assetName = vc.Attribute("assetName").Value;
					string nextLevel = vc.Attribute("nextLevel").Value;
					((ExitVictoryCondition)instance).ExitAssetName = assetName;
					((ExitVictoryCondition)instance).NextLevel = nextLevel;
					this.VictoryConditions.Add((ExitVictoryCondition)instance);
				}
			}
		}

		public void CheckVictoryConditions()
		{
			bool victory = (this.VictoryConditions.Count > 0);
			foreach (var condition in this.VictoryConditions)
			{
				victory &= condition.IsConditionComplete(this);
			}

			LevelCompleted = victory;
		}

		/// <summary>
		/// Init Hulls : each floor tile with IsWalkable property set to false will cast shadow by default
		/// </summary>
		/// <param name="penumbra"></param>
		/// <param name="layers"></param>
		private void InitPenumbraHulls(PenumbraComponent penumbra, Dictionary<float, SpriteBase[,]> layers)
		{
			//Get floor layer
			_layers.TryGetValue(0f, out SpriteBase[,] floor);


			// TODO: add castShadow property to floor layer to enable non walkable areas not to cast shadow
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

			Hull newHull = new Hull(position, new Vector2(x + pix, y), new Vector2(x + pix, y + pix), new Vector2(x, y + pix));
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
				if (actor.Attribute("class")== null)
					break;

				//CreateInstance class
				var hndl = Activator.CreateInstance(assemblyName, assemblyName + "." + actor.Attribute("class").Value);
				var instance = hndl.Unwrap();

				//Get texture
				_textures.TryGetValue(actor.Attribute("assetName").Value, out Texture2D texture);

				int.TryParse(tile.Attribute("posX").Value, out int posX);
				int.TryParse(tile.Attribute("posY").Value, out int posY);

				((SpriteBase)instance).Texture = texture;
				((SpriteBase)instance).Position = new Vector2(posX * pix, posY * pix);
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

				//Actor FloorTileSpawner logic
				if (instance.GetType() == typeof(LavaTileSpawner))
				{
					InitActorFloorTileSpawner(actor, instance);
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
				Actors.Add((SpriteBase)instance);
			}
		}

		private void InitActorFloorTileSpawner(XElement actor, object instance)
		{
			var spawner = ((LavaTileSpawner)instance);

			Layers.TryGetValue(0f, out SpriteBase[,] floor);
			spawner.Floor = floor;

			int.TryParse(actor.Attribute("interval").Value, out int interval);
			spawner.Interval = interval;

			bool.TryParse(actor.Attribute("isWalkable").Value, out bool isWalkable);
			spawner.IsWalkable = isWalkable;

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

			if(actor.Attribute("pickupSound") != null)
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
					grid[posY, posX] = new Floor(texture, new Vector2(posX * pix, posY * pix), null, depth, isWalkable);
				}
				else
				{
					grid[posY, posX] = new SpriteBase(texture, new Vector2(posX * pix, posY * pix), null, 0f, Color.White, depth);
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
		/// Draw level : tiles, player and actors
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
			foreach (var actor in Actors)
			{
				actor.Draw(gameTime, spriteBatch);
			}
		}

		/// <summary>
		/// Level Update logic
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="sprites"></param>
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
			UpdatePlayer(gameTime, sprites, _layers);

			//Update actors
			UpdateActors(gameTime, sprites, _layers);

			//Check for collisions between player and actors after movement
			CheckCollisions();

			//Remove dead actors from actors
			Actors.RemoveAll(m => m.IsAlive == false);
		}

		/// <summary>
		/// Check collisions between player and actors, 
		/// and between actors themselves.
		/// </summary>
		private void CheckCollisions()
		{
			foreach (var actor in Actors)
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

						//play pickup sound
						if (!string.IsNullOrEmpty(item.PickupSound))
						{
							_soundEffects.TryGetValue(item.PickupSound, out SoundEffect sound);
							sound.CreateInstance().Play();
						}

						actor.IsAlive = false;
						RemoveLightAtPosition(actor.Position);
					}

					if (actor.GetType() == typeof(Monster))
					{
						var monster = ((Monster)actor);

						_player.Health -= Math.Max(monster.Attack - _player.Defense, 0);
						_player.Score += monster.Score;

						//wearing armor
						_player.Defense -= Math.Max(monster.Attack / 3, 0);

						actor.IsAlive = false;
					}

					if (actor.GetType() == typeof(LavaTileSpawner))
					{
						//instant death
						Player.Health = 0;
					}
				}
			}

			//Lava consumes us all !!
			foreach (var lava in Actors.Where(a => a.GetType() == typeof(LavaTileSpawner)))
			{
				foreach (var other in Actors.Where(a => a.GetType() != typeof(LavaTileSpawner)))
				{
					if (lava.Position.Equals(other.Position))
						other.IsAlive = false;
				}
			}

			//floor player health
			_player.Health = Math.Max(_player.Health, 0);
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

		private void UpdateActors(GameTime gameTime, List<SpriteBase> sprites, Dictionary<float, SpriteBase[,]> layers)
		{
			layers.TryGetValue(0f, out SpriteBase[,] floor);

			foreach (var actor in Actors.ToArray())
			{
				if (actor.GetType() == typeof(Monster))
				{
					((Monster)actor).Update(gameTime, Actors, floor);
				}
				else if (actor.GetType() == typeof(LavaTileSpawner))
				{
					((LavaTileSpawner)actor).Update(gameTime, Actors, _penumbra);
				}
				else
				{
					actor.Update(gameTime, Actors);
				}
			}
		}

		private void UpdatePlayer(GameTime gameTime, List<SpriteBase> sprites, Dictionary<float,SpriteBase[,]> layers )
		{

			layers.TryGetValue(0f, out SpriteBase[,] floor);

			State prevState = _player.MoveState;
			_player.Update(gameTime, sprites, floor);

			//if player has moved, remove 1 HP
			if (prevState != State.Iddle && _player.MoveState == State.Iddle)
				_player.Health -= 1;
		}
	}
}
