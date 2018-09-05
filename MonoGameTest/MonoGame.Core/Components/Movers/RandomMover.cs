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

		public void Move(SpriteBase sprite, SpriteBase[,] floor, GameTime gameTime)
		{
			if (gameTime == null || gameTime.TotalGameTime.TotalSeconds - _timer < _interval)
			{
				return;
			}

			//Move
			_timer = gameTime.TotalGameTime.TotalSeconds;

			//Iddle - check input
			if (sprite.MoveState == State.Iddle)
			{

				float targetX = sprite.Position.X;
				float targetY = sprite.Position.Y;

				int direction = _random.Next(0, 4);

				switch (direction)
				{
					case 0:
						targetY -= _moveSpeed;
						sprite.MoveState = State.MovingUp;
						break;
					case 1:
						targetY += _moveSpeed;
						sprite.MoveState = State.MovingDown;
						break;
					case 2:
						targetX -= _moveSpeed;
						sprite.MoveState = State.MovingLeft;
						break;
					case 3:
						targetX += _moveSpeed;
						sprite.MoveState = State.MovingRight;
						break;
				}

				sprite.SpriteDestination = new Vector2(targetX, targetY);
			}

			

			//Check if movement is possible
			var destinationTile = floor[sprite.GridDestinationX, sprite.GridDestinationY] as Floor;

			if (sprite.MoveState != State.Iddle && destinationTile != null && destinationTile.IsWalkable)
			{

				float newX = sprite.Position.X;
				float newY = sprite.Position.Y;
				float speed = 0.15f;

				//Pixels per cycle
				double amount = speed * gameTime.ElapsedGameTime.TotalMilliseconds;

				switch (sprite.MoveState)
				{
					case State.MovingUp:
						if (sprite.Position.Y - (amount) <= sprite.SpriteDestination.Y)
						{
							sprite.Position = sprite.SpriteDestination;
							sprite.MoveState = State.Iddle;
						}
						else
							newY -= (int)(amount);
						break;

					case State.MovingDown:
						if (sprite.Position.Y + (amount) >= sprite.SpriteDestination.Y)
						{
							sprite.Position = sprite.SpriteDestination;
							sprite.MoveState = State.Iddle;
						}
						else
							newY += (int)(amount);
						break;

					case State.MovingLeft:
						if (sprite.Position.X - (amount) <= sprite.SpriteDestination.X)
						{
							sprite.Position = sprite.SpriteDestination;
							sprite.MoveState = State.Iddle;
						}
						else
							newX -= (int)(speed * gameTime.ElapsedGameTime.TotalMilliseconds);
						break;

					case State.MovingRight:
						if (sprite.Position.X + (amount) >= sprite.SpriteDestination.X)
						{
							sprite.Position = sprite.SpriteDestination;
							sprite.MoveState = State.Iddle;
						}
						else
							newX += (int)(amount);
						break;
				}

				//Set new sprite position
				if (sprite.MoveState != State.Iddle)
					sprite.Position = new Vector2(newX, newY);

			}
			else
			{
				sprite.MoveState = State.Iddle;
			}
		}
	}
}
