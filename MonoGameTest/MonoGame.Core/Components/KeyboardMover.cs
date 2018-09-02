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

		public Vector2 GetNewPosition(SpriteBase sprite, GameTime gametime = null)
		{

			Vector2 newPosition = sprite.Position;
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

			_previousState = state;

			return newPosition;

		}
	}
}
