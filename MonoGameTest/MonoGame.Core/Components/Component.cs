using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public abstract class Component
	{

		public abstract void Update(GameTime gameTime, List<SpriteBase> sprites);

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

	}
}
