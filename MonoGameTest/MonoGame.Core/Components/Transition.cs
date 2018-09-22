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
		private Action _action;

		private int _growMilliseconds = 2000;
		private int _waitMilliseconds = 1000;
		private int _shrinkMilliseconds = 2000;

		private bool _transitionEnd = false;

		public Transition(Action onTransitionEnd)
		{
			this._action = onTransitionEnd;
		}

		//Draw transitions object
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		//Update transition objects
		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{




			//Transition ended


			if (_transitionEnd && _action != null)
				_action();
		}
	}
}
