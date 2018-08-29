using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core.Wrappers
{
	public class GamePadInputWrapper : IInputWrapper
	{
		public MoveDirection GetMoveDirection()
		{
			return MoveDirection.None;
		}
	}
}
