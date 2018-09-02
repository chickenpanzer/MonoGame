using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class SpriteBase : Component , ICloneable
	{

		#region Private fields
		protected Texture2D _spriteTexture = null;
		protected Vector2 _spritePosition;
		protected IMoveable _mover = null;

		protected int _spriteHeight;
		protected int _spriteWidth;

		protected bool _isAlive;

		protected Color _color = Color.White;
		protected float _layer = 0f;

		protected float _textureScale = 1f;
		#endregion

		#region Public Properties
		public float MoveSpeed { get; set; }
		public float TextureScale
		{
			get { return _textureScale; }
			set
			{
				_textureScale = value;
				_spriteHeight = (int)(_spriteTexture.Height * _textureScale);
				_spriteWidth = (int)(_spriteTexture.Width * _textureScale);
			}
		}

		public Vector2 Position { get { return _spritePosition; } set { _spritePosition = value; } }
		public Texture2D Texture { get { return _spriteTexture; } set { _spriteTexture = value; } }

		public int SpriteHeight { get => _spriteHeight; }
		public int SpriteWidth { get => _spriteWidth; }

		//IMovable access
		public IMoveable Mover { get => _mover; set => _mover = value; }

		public bool IsAlive
		{
			get { return _isAlive; }
			set { _isAlive = value; }
		}

		public float Layer { get => _layer; set => _layer = value; }
		#endregion

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_spriteTexture, _spritePosition, null, _color, 0f, Vector2.Zero, TextureScale, SpriteEffects.None, _layer);
		}

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//Move Sprite
			if (_mover != null)
				_spritePosition = _mover.GetNewPosition(this.Position, gameTime);
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}

		#region Constructors
		public SpriteBase()
		{
			_isAlive = true;
		}

		public SpriteBase(Texture2D texture, Vector2 position, IMoveable mover,float moveSpeed)
		{
			MoveSpeed = moveSpeed;
			_spriteTexture = texture ?? throw new ArgumentNullException("texture");
			_mover = mover;
			_spritePosition = position;
			_isAlive = true;
		}

		public SpriteBase(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color)
		{
			MoveSpeed = moveSpeed;
			_spriteTexture = texture ?? throw new ArgumentNullException("texture");
			_mover = mover;
			_spritePosition = position;
			_color = color;
			_isAlive = true;
		}

		public SpriteBase(Texture2D texture, Vector2 position, IMoveable mover, float moveSpeed, Color color, float layer)
		{
			MoveSpeed = moveSpeed;
			_spriteTexture = texture ?? throw new ArgumentNullException("texture");
			_mover = mover;
			_spritePosition = position;
			_color = color;
			_layer = layer;
			_isAlive = true;
		}
		#endregion	
	}
}
