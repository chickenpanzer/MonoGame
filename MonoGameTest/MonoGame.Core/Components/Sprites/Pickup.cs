using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core
{
	public class Pickup : SpriteBase
	{
		public int HealthValue { get; set; }
		public int ScoreValue { get; set; }
		public int AttackValue { get; set; }
		public int DefenseValue { get; set; }
		public string PickupSound { get; set; }

		public Pickup()
		{

		}

		public Pickup(int healthValue, int scoreValue, int attackValue, int defenseValue)
		{
			HealthValue = healthValue;
			ScoreValue = scoreValue;
			AttackValue = attackValue;
			DefenseValue = defenseValue;
		}

		public override void Update(GameTime gameTime, List<SpriteBase> sprites)
		{
			return;
		}

	}
}
