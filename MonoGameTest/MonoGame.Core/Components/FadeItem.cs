using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class FadeItem
	{

		public float PosX { get; set; }
		public float PosY { get; set; }
		public bool IsAlive { get; private set; }

		public float Delay { get; set; }
		public float Radians { get; private set; }

		public FadeItem()
		{
			IsAlive = true;
		}

		public float Scale
		{
			get
			{
				if (Delay > 0)
					return 2.0f;
				else if (Radians > MathHelper.Pi)
					return 0f;

				return (float)Math.Cos(Radians) + 1;

			}
		}

		public void Update(float deltaTimeInMilliseconds)
		{
			Delay -= deltaTimeInMilliseconds;

			if (Delay < 0)
				Radians += deltaTimeInMilliseconds / 300.0f;

			IsAlive = !(Scale == 0f);
		}
	}
}
