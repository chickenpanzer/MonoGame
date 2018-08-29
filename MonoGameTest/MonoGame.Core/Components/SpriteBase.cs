using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class SpriteBase : Component
	{

		private Texture2D _spriteTexture = null;
		private Vector2 _spritePosition;
		private IInputWrapper _inputWrapper = null;

		public float MoveSpeed { get; set; }



		private bool _isAlive;

		public bool IsAlive
		{
			get { return _isAlive; }
			set { _isAlive = value; }
		}


		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_spriteTexture, _spritePosition, Color.White);
		}

		public override void Update(GameTime gameTime)
		{
			if (_inputWrapper == null)
			{
				return;
			}

			MoveDirection move = _inputWrapper.GetMoveDirection();

			switch (move)
			{
				case MoveDirection.Top:
					_spritePosition.Y -= MoveSpeed;
					break;

				case MoveDirection.Right:
					_spritePosition.X += MoveSpeed;
					break;

				case MoveDirection.Bottom:
					_spritePosition.Y += MoveSpeed;
					break;

				case MoveDirection.Left:
					_spritePosition.X -= MoveSpeed;
					break;

				case MoveDirection.TopRight:
					_spritePosition.X += MoveSpeed;
					_spritePosition.Y -= MoveSpeed;
					break;

				case MoveDirection.BottomRight:
					_spritePosition.X += MoveSpeed;
					_spritePosition.Y += MoveSpeed;
					break;

				case MoveDirection.TopLeft:
					_spritePosition.X -= MoveSpeed;
					_spritePosition.Y -= MoveSpeed;
					break;

				case MoveDirection.BottomLeft:
					_spritePosition.X -= MoveSpeed;
					_spritePosition.Y += MoveSpeed;
					break;
			}

		}

		public SpriteBase(Texture2D texture, Vector2 position, IInputWrapper wrapper, float moveSpeed)
		{
			MoveSpeed = moveSpeed;
			_spriteTexture = texture ?? throw new ArgumentNullException("texture");
			_inputWrapper = wrapper;
			_spritePosition = position;
		}
	}
}
