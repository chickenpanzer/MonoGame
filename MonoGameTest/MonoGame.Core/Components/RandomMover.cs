using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGame.Core
{
	public class RandomMover : IMoveable
	{

		private float _moveSpeed;
		private double _timer = 0;
		private double _interval = 0;
		private Random _random = new Random();

		public RandomMover(float moveSpeed, double moveInterval)
		{
			_moveSpeed = moveSpeed;
			_interval = moveInterval;
		}

		public Vector2 GetNewPosition(SpriteBase sprite, GameTime gametime = null)
		{
			Vector2 newPosition = sprite.Position;

			if (gametime == null || gametime.TotalGameTime.TotalMilliseconds - _timer < _interval)
			{
				//Don't move
				return newPosition;
			}
			
			//Move
			_timer = gametime.TotalGameTime.TotalMilliseconds;

			int verticalDirection = _random.Next(-1, 2);
			int horizontalDirection = _random.Next(-1, 2);

			newPosition.X += horizontalDirection * _moveSpeed;
			newPosition.Y += verticalDirection * _moveSpeed;

			//Prevent offScreen Movement
			if (newPosition.X < 0)
				newPosition.X = 0;

			if (newPosition.Y < 0)
				newPosition.Y = 0;

			if (newPosition.X + sprite.SpriteWidth > Constants.ScreenWidth)
			{
				while (newPosition.X + sprite.SpriteWidth > Constants.ScreenWidth)
					newPosition.X -= 1;
			}

			if (newPosition.Y + sprite.SpriteHeight > Constants.ScreenHeight)
			{
				while (newPosition.Y + sprite.SpriteHeight > Constants.ScreenHeight)
					newPosition.Y -= 1;
			}

			return newPosition;
		}
	}
}
