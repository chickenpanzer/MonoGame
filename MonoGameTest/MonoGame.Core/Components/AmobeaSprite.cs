using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class AmobeaSprite : SpriteBase
	{
		private double _timer = 0;
		private int _interval = 5;

		private Random _random = new Random(); 

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//Move Sprite
			if (_mover != null)
				_spritePosition = _mover.GetNewPosition(this, gameTime);

			//Spawn new amobea
			SpawnAmobea(gameTime, sprites);
		}

		private void SpawnAmobea(GameTime gameTime, List<SpriteBase> sprites)
		{
			if (gameTime == null || gameTime.TotalGameTime.TotalSeconds - _timer < _interval)
			{
				//Don't spawn
				return;
			}

			//spawn new sprite
			Debug.Print("Spawning...");
			_timer = gameTime.TotalGameTime.TotalSeconds;

			//33% chance of spawning a new amobea
			int res = _random.Next(0, 101);
			if (res > 66)
			{
				Debug.Print("=> Amobea");
				var clone = this.Clone() as AmobeaSprite;
				clone.Mover = new RandomMover(1f, 2000);
				sprites.Add(clone);
			}
			else
			{
				Debug.Print("=>SpriteBase");
				var child = new SpriteBase(this._spriteTexture, new Vector2(this._spritePosition.X + 50, this._spritePosition.Y + 50), new RandomMover(2f, 1), 10f, Color.White, 0.3f);
				child.TextureScale = 0.5f;
				sprites.Add(child);
			}

			Debug.Print(string.Format("Sprites in list : {0}", sprites.Count));
		}

		public AmobeaSprite(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed) : base(texture, position, mover, moveSpeed)
		{
		}

		public AmobeaSprite(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color) : base(texture, position, mover, moveSpeed, color)
		{
		}

		public AmobeaSprite(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color, float layer) : base(texture, position, mover, moveSpeed, color,layer)
		{
		}
	}
}
