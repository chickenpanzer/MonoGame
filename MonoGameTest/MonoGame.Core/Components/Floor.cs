using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Floor : SpriteBase
	{

		private bool _isWalkable;

		public bool IsWalkable
		{
			get { return _isWalkable; }
			set { _isWalkable = value; }
		}



		public Floor()
		{

		}

		public Floor(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, bool walkable) : base(texture, position, mover, moveSpeed)
		{
			IsWalkable = walkable;
		}
	}
}
