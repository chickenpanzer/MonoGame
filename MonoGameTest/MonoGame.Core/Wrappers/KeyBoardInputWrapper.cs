using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class KeyBoardInputWrapper : IInputWrapper
	{
		public MoveDirection GetMoveDirection()
		{
			KeyboardState state = Keyboard.GetState();

			if (state.GetPressedKeys().Length > 2 || state.GetPressedKeys().Length == 0)
			{
				return MoveDirection.None;
			}
			else
			{
				MoveDirection move = MoveDirection.None;

				foreach (var key in state.GetPressedKeys())
				{
					switch (key)
					{
						case Keys.Up:
							move = Combine(move, MoveDirection.Top);
							break;
						case Keys.Right:
							move = Combine(move, MoveDirection.Right);
							break;
						case Keys.Down:
							move = Combine(move, MoveDirection.Bottom);
							break;
						case Keys.Left:
							move = Combine(move, MoveDirection.Left);
							break;
						default:
							break;
					}
				}

				return move;
			}
		}

		private MoveDirection Combine(MoveDirection buffer, MoveDirection newMove)
		{

			MoveDirection result = buffer;

			if (result == MoveDirection.None)
				return newMove;

			switch (newMove)
			{
				case MoveDirection.Top:
					result = (result == MoveDirection.Left) ? MoveDirection.TopLeft : result;
					result = (result == MoveDirection.Right) ? MoveDirection.TopRight : result;
					break;

				case MoveDirection.Right:
					result = (result == MoveDirection.Top) ? MoveDirection.TopRight : result;
					result = (result == MoveDirection.Bottom) ? MoveDirection.BottomRight : result;
					break;

				case MoveDirection.Bottom:
					result = (result == MoveDirection.Right) ? MoveDirection.BottomRight : result;
					result = (result == MoveDirection.Left) ? MoveDirection.BottomLeft : result;
					break;

				case MoveDirection.Left:
					result = (result == MoveDirection.Top) ? MoveDirection.TopLeft : result;
					result = (result == MoveDirection.Bottom) ? MoveDirection.BottomLeft : result;
					break;

				default:
					break;
			}

			return result;

		}
	}
}