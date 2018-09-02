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
		private int _interval = 0;
		private Random _random = new Random();

		public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
		public int Interval { get => _interval; set => _interval = value; }

		public RandomMover()
		{
			_moveSpeed = 0f;
			_interval = 1;
		}

		public RandomMover(float moveSpeed, int moveInterval)
		{
			_moveSpeed = moveSpeed;
			_interval = moveInterval;
		}

		public Vector2 GetNewPosition(Vector2 actual, GameTime gametime = null)
		{
			Vector2 newPosition = actual;

			if (gametime == null || gametime.TotalGameTime.TotalSeconds - _timer < _interval)
			{
				//Don't move
				return newPosition;
			}
			
			//Move
			_timer = gametime.TotalGameTime.TotalSeconds;

			int verticalDirection = _random.Next(-1, 2);
			int horizontalDirection = _random.Next(-1, 2);

			newPosition.X += horizontalDirection * _moveSpeed;
			newPosition.Y += verticalDirection * _moveSpeed;

			//Prevent offScreen Movement
			if (newPosition.X < 0)
				newPosition.X = 0;

			if (newPosition.Y < 0)
				newPosition.Y = 0;

			// TODO: check this
			if (newPosition.X > Constants.ScreenWidth)
			{
				while (newPosition.X > Constants.ScreenWidth)
					newPosition.X -= 1;
			}

			if (newPosition.Y > Constants.ScreenHeight)
			{
				while (newPosition.Y > Constants.ScreenHeight)
					newPosition.Y -= 1;
			}

			return newPosition;
		}

	}
}
