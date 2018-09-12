using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Penumbra;

namespace MonoGame.Core
{
	public class LavaTileSpawner : SpriteBase
	{

		private static int max = 5000;
		private static int current = 0;


		private static double _timer;
		private int _interval;

		private SpriteBase[,] _floor;

		public SpriteBase[,] Floor
		{
			get { return _floor; }
			set { _floor = value; }
		}

		public int Interval { get => _interval; set => _interval = value; }
		public bool IsWalkable { get; internal set; }


		/// <summary>
		/// For every interval, check surrounding tiles and spawn one FloorTileSpawner per available tile (walkable)
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="sprites"></param>
		public void Update(GameTime gameTime, List<SpriteBase> sprites, PenumbraComponent penumbra)
		{

			if (gameTime == null || gameTime.TotalGameTime.TotalSeconds - _timer < _interval)
			{
				return;
			}

			//Spawn
			_timer = gameTime.TotalGameTime.TotalSeconds;

			SpawnOnTile(Position.X, Position.Y - 32, sprites,penumbra);
			SpawnOnTile(Position.X, Position.Y + 32, sprites, penumbra);
			SpawnOnTile(Position.X - 32, Position.Y, sprites, penumbra);
			SpawnOnTile(Position.X + 32, Position.Y, sprites, penumbra);


			base.Update(gameTime, sprites);
		}


		private void SpawnOnTile(float posX, float posY, List<SpriteBase> sprites, PenumbraComponent penumbra)
		{

			try
			{
				Floor tile = null;
				Vector2 targetPosition = new Vector2(posX, posY);

				foreach (var ft in Floor)
				{
					if (ft.Position.Equals(targetPosition))
					{
						tile = ft as Floor;
						break;
					}
				}

				if (tile != null && tile.IsWalkable && tile.Texture.Name != this.Texture.Name && current < max)
				{
					var newSpawner = new LavaTileSpawner()
					{
						Floor = this.Floor,
						Interval = this.Interval,
						Position = tile.Position,
						Texture = this.Texture,
						IsWalkable = this.IsWalkable
					};

					current++;

					//Change floor texture and property
					tile.Texture = this.Texture;
					tile.IsWalkable = this.IsWalkable;

					//Add new spawner à position
					sprites.Add(newSpawner);

					//Add new light at position
					penumbra.Lights.Add(new PointLight
					{
						Scale = new Vector2(100f,100f), // Range of the light source (how far the light will travel)
						ShadowType = ShadowType.Illuminated, // Will not lit hulls themselves
						Color = Color.MonoGameOrange,
						Position = new Vector2(tile.Position.X + 16, tile.Position.Y + 16)
					});

					//Remove self
					IsAlive = false;
				}
				else
				{
					//Remove self
					IsAlive = false;
				}

			}
			catch (Exception ex)
			{
			}
	
		}
	}
}
