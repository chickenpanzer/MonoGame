using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{
	public class PickAllVictoryCondition : IVictoryCondition
	{

		private string _assetName;

		public string PickupAssetName
		{
			get { return _assetName; }
			set { _assetName = value; }
		}

		/// <summary>
		/// Check in actors for a particuliar asset name
		/// Condition is complete if no actor has this assetName (all assets have been picked)
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public bool IsConditionComplete(Level level)
		{
			var actor = level.Actors.Where(a => a.Texture.Name == PickupAssetName).FirstOrDefault();
			return actor == null;
		}
	}
}
