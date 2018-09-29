using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Transition : Component
	{
		private Action _onTransitionEndAction;
		private Action _loadLevel;

		private Texture2D _texture;

		private List<FadeItem> _fadeItems = new List<FadeItem>();

		private bool _transitionEnd = false;
		private bool _transitionStart = false;
		private float _transitionStartScale = 0f;
		private int _startDelay = 20;

		/// <summary>
		/// Reset transition in initial State
		/// </summary>
		public void Reset()
		{
			if (_transitionEnd)
			{
				InitFadeItems();
				_transitionStart = true;
				_transitionStartScale = 0f;
				_transitionEnd = false;
			}
		}

		public Transition(Action onTransitionEnd, Action loadLevel,Texture2D texture)
		{
			this._onTransitionEndAction = onTransitionEnd;
			this._loadLevel = loadLevel;
			this._texture = texture;
			_transitionEnd = true;

		}

		/// <summary>
		/// Initi transition fade items
		/// </summary>
		private void InitFadeItems()
		{
			for (int i = 0; i < Constants.ScreenWidth; i += 64)
			{
				for (int y = 0; y < Constants.ScreenHeight; y += 64)
				{
					_fadeItems.Add(new FadeItem()
					{
						PosX = i,
						PosY = y,
						Delay = i + y
					});
				}
			}
		}

		//Draw transitions object
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (_transitionStart)
			{
				spriteBatch.Draw(_texture, new Vector2(-300,-300), null, Color.MonoGameOrange, 0f, new Vector2(1, 1), _transitionStartScale, SpriteEffects.None, 0f);
			}
			else
			{
				foreach (var fadeItem in _fadeItems)
				{
					spriteBatch.Draw(_texture, new Vector2(fadeItem.PosX - 16, fadeItem.PosY - 16), null, Color.MonoGameOrange, 0f, new Vector2(1, 1), fadeItem.Scale, SpriteEffects.None, 0f);
				}
			}
		}

		//Update transition objects
		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			//Expand phase
			if (_transitionStart)
			{
				if (_transitionStartScale < 30f)
				{
					_startDelay -= gameTime.ElapsedGameTime.Milliseconds;
					if (_startDelay <= 0)
					{
						_transitionStartScale += 0.5f;
						_startDelay = 10;
					}
				}
				else //Shrink phase
				{
					_transitionStart = false;
					_loadLevel?.Invoke();
				}

			}
			else
			{
				foreach (var fadeItem in _fadeItems)
				{
					fadeItem.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
				}
			}

			//Remove dead objects
			_fadeItems.RemoveAll(fi => !fi.IsAlive);

			//Transition ended
			_transitionEnd = _fadeItems.Count() == 0;

			//Call external action on transition end
			if (_transitionEnd && _onTransitionEndAction != null)
				_onTransitionEndAction();
		}
	}
}
