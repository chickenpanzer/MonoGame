using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public interface IMoveable
	{

		float MoveSpeed { get; set; }
		int Interval { get; set; }

		Vector2 GetNewPosition(SpriteBase sprite, GameTime gametime = null);

	}
}
