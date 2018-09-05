using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Player : SpriteBase, IActor
	{
		private int _defense;

		public int Health { get; set; }
		public int Score { get; set; }
		public int Attack { get; set; }
		public int Defense
		{
			get { return _defense; }

			set
			{
				_defense = value;
				if (_defense < 0)
					_defense = 0;
			}
		}

		public Player()
		{
			
		}

		public Player(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed) : base(texture, position, mover, moveSpeed)
		{
			Health = 100;
			Score = 0;
			Attack = 10;
			Defense = 0;
		}

		public void Update(GameTime gameTime, List<SpriteBase> sprites, SpriteBase[,] floor)
		{
			//Move Sprite
			if (_mover != null)
				_mover.Move(this, floor, gameTime);
		}

		public override string ToString()
		{
			return string.Format("PosX={0} : PosY={1} : Health={2} : Score={3} : Attack={4} : Defense={5}", Position.X, Position.Y, Health, Score, Attack, Defense);
		}
	}
}
