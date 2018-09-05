using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.Core
{
	public class KeyboardMover : IMoveable
	{

		private float _moveSpeed = 0f;
		private bool _isReverse;
		private KeyboardState _previousState;

		public KeyboardMover()
		{
			_moveSpeed = 0f;
			_isReverse = false;
			_previousState = Keyboard.GetState();
		}

		public KeyboardMover(float moveSpeed, bool reverse = false)
		{
			_moveSpeed = moveSpeed;
			_isReverse = reverse;
			_previousState = Keyboard.GetState();
		}

		public float MoveSpeed { get => _moveSpeed; set { _moveSpeed = value; } }
		public int Interval { get => 0; set {;} }

		public Vector2 GetNewPosition(Vector2 actual, GameTime gametime = null)
		{

			Vector2 newPosition = actual;
			float moveValue = 0f;

			moveValue = (_isReverse ? _moveSpeed * -1 : _moveSpeed);

			KeyboardState state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.Up) && _previousState.IsKeyUp(Keys.Up))
				newPosition.Y -= moveValue;

			if (state.IsKeyDown(Keys.Down) && _previousState.IsKeyUp(Keys.Down))
				newPosition.Y += moveValue;

			if (state.IsKeyDown(Keys.Right) && _previousState.IsKeyUp(Keys.Right))
				newPosition.X += moveValue;

			if (state.IsKeyDown(Keys.Left) && _previousState.IsKeyUp(Keys.Left))
				newPosition.X -= moveValue;


			//////Prevent offScreen Movement
			////if (newPosition.X < 0)
			////	newPosition.X = 0;

			////if (newPosition.Y < 0)
			////	newPosition.Y = 0;

			////if (newPosition.X  > Constants.ScreenWidth)
			////{
			////	while (newPosition.X  > Constants.ScreenWidth)
			////		newPosition.X -= 1;
			////}

			////if (newPosition.Y  > Constants.ScreenHeight)
			////{
			////	while (newPosition.Y  > Constants.ScreenHeight)
			////		newPosition.Y -= 1;
			////}

			_previousState = state;

			return newPosition;

		}

		public void Move(SpriteBase sprite, SpriteBase[,] floor, GameTime gameTime)
		{
			float moveValue = (_isReverse ? _moveSpeed * -1 : _moveSpeed);

			//Iddle - check input
			if (sprite.MoveState == State.Iddle)
			{
				KeyboardState state = Keyboard.GetState();

				float targetX = sprite.Position.X;
				float targetY = sprite.Position.Y;

				if (state.GetPressedKeys().Length == 1)
				{
					if (state.IsKeyDown(Keys.Up))
					{
						targetY -= moveValue;
						sprite.MoveState = State.MovingUp;
					}

					if (state.IsKeyDown(Keys.Down))
					{
						targetY += moveValue;
						sprite.MoveState = State.MovingDown;
					}

					if (state.IsKeyDown(Keys.Right))
					{
						targetX += moveValue;
						sprite.MoveState = State.MovingRight;
					}

					if (state.IsKeyDown(Keys.Left))
					{
						targetX -= moveValue;
						sprite.MoveState = State.MovingLeft;
					}

					sprite.SpriteDestination = new Vector2(targetX, targetY);
				}
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
				if(sprite.MoveState != State.Iddle)
					sprite.Position = new Vector2(newX, newY);

			}
			else
			{
				sprite.MoveState = State.Iddle;
			}
		}
	}
}
