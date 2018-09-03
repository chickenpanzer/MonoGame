using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public interface IVictoryCondition
	{

		bool IsConditionComplete(Level level);

	}
}
