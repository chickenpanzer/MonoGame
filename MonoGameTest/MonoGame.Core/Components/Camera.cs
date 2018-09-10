using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class Camera
	{
		public Matrix Transform { get; private set; }

		public void Follow(SpriteBase target)
		{

			var position = Matrix.CreateTranslation(
			-target.Position.X - 16,
			-target.Position.Y - 16,
			0);

			var offset = Matrix.CreateTranslation(
			Constants.ScreenWidth / 2,
			Constants.ScreenHeight / 2,
			0);

			Transform = position * offset;
		}
	}
}
