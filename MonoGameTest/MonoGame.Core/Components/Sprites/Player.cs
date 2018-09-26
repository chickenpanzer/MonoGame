using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Player : SpriteBase, IActor
	{
		#region private fields
		private const int spriteSize = 32;
		private int _defense;

		private int _currentFrame = 0;
		private int _delayBetweenFrames = 200;
		private int _MillisecondsSinceLastFrame;
		private int _increment = 1;

		private int _xSprite = 0;
		private int _ySprite = 0;
		#endregion

		public int Health { get; set; }
		public int Score { get; set; }
		public int Attack { get; set; }
		public int Defense
		{
			get { return _defense; }

			set
			{
				_defense = value;
				if (_defense < 0)
					_defense = 0;
			}
		}

		public Player()
		{
			
		}

		public Player(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed) : base(texture, position, mover, moveSpeed)
		{
			Health = 100;
			Score = 0;
			Attack = 10;
			Defense = 0;
		}

		public void Update(GameTime gameTime, List<SpriteBase> sprites, SpriteBase[,] floor)
		{
			//Move Sprite
			if (_mover != null)
				_mover.Move(this, floor, gameTime);

			//Fixing x and y sprite to use
			switch (this.MoveState)
			{
				case State.Iddle:
					//Debug.Print("iddle");
					break;

				case State.MovingUp:
					_xSprite = GetNextFrame(_currentFrame, gameTime);
					_ySprite = 3;
					break;

				case State.MovingDown:
					_xSprite = GetNextFrame(_currentFrame, gameTime);
					_ySprite = 0;
					break;

				case State.MovingRight:
					_xSprite = GetNextFrame(_currentFrame, gameTime);
					_ySprite = 2;
					break;

				case State.MovingLeft:
					_xSprite = GetNextFrame(_currentFrame, gameTime);
					_ySprite = 1;
					break;
			}

			_currentFrame = _xSprite;

		}

		//Draw override : draw correct image in spritesheet according to movement direction and timing
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{


			//Using source rectangle to determine spritesheet area to render in destination rectangle
			Rectangle source = new Rectangle(_xSprite * spriteSize, _ySprite * spriteSize, spriteSize, spriteSize);
			Rectangle destination = new Rectangle((int)this.Position.X, (int)this.Position.Y, spriteSize, spriteSize);

			spriteBatch.Draw(this.Texture, destination, source, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

		}

		private int GetNextFrame(int currentFrame, GameTime gameTime)
		{
			int frame = currentFrame;

			//Change Frame
			if (_MillisecondsSinceLastFrame > _delayBetweenFrames)
			{
				//Back and forth
				if (frame == 2)
					_increment = -1;

				if (frame == 0)
					_increment = 1;

				frame += _increment;

				//reset timer
				_MillisecondsSinceLastFrame = 0;
			}
			else
			{
				//Increment timer
				_MillisecondsSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
			}

			return frame;			
		}

		public override string ToString()
		{
			return string.Format("PosX={0} : PosY={1} : Health={2} : Score={3} : Attack={4} : Defense={5}", Position.X, Position.Y, Health, Score, Attack, Defense);
		}
	}
}
