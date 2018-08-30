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

		Vector2 GetNewPosition(SpriteBase sprite, GameTime gametime = null);

	}
}
