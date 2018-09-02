using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public interface IActor
	{
		int Health { get; set; }
		int Score { get; set; }
		int Attack { get; set; }
		int Defense { get; set; }
	}
}
