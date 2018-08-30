using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class AmobeaSprite : SpriteBase
	{

		private List<SpriteBase> _childrens = new List<SpriteBase>();
		private int _timer = 0;
		private int _interval = 5;

		public List<SpriteBase> Childrens { get => _childrens; }

		public override void Update(GameTime gameTime)
		{
			//Move Sprite
			if (_mover != null)
				_spritePosition = _mover.GetNewPosition(this, gameTime);

			//Spawn new amobea
			SpawnAmobea(gameTime);

			//Move childrens
			foreach (var child in _childrens)
			{
				child.Update(gameTime);
			}

			
		}

		private void SpawnAmobea(GameTime gameTime)
		{
			if (gameTime == null || gameTime.TotalGameTime.Seconds - _timer < _interval)
			{
				//Don't spawn
				return;
			}

			//spawn
			_timer = gameTime.TotalGameTime.Seconds;
			var child = new SpriteBase(this._spriteTexture, new Vector2(200f,200f), null, 10f,Color.White,2f);
			child.TextureScale = 0.5f;
			_childrens.Add(child);


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
