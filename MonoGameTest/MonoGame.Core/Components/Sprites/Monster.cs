using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class Monster : SpriteBase, IActor
	{
		private Color color;

		public int Health { get; set; }
		public int Score { get; set; }
		public int Attack { get; set; }
		public int Defense { get; set; }

		public Monster()
		{

		}

		public Monster(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed) : base(texture, position, mover, moveSpeed)
		{
			Health = 100;
			Score = 0;
			Attack = 10;
			Defense = 0;
		}

		public Monster(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color) : this(texture, position, mover, moveSpeed)
		{
			this.color = color;
		}

		public Monster(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color, float layer) : this(texture, position, mover, moveSpeed, color)
		{
			Layer = layer;
		}

		public override string ToString()
		{
			return string.Format("PosX={0} : PosY={1} : Health={2} : Score={3} : Attack={4} : Defense={5}", Position.X, Position.Y, Health, Score, Attack, Defense);
		}
	}

}
