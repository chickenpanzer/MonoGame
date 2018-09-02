using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Player : SpriteBase
	{
		
		public Player()
		{

		}

		public Player(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed) : base(texture, position, mover, moveSpeed)
		{
			
		}
	}
}
