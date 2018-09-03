using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Core
{

	public class PickOneVictoryCondition : IVictoryCondition
	{
		private string _assetName;

		public string PickupAssetName
		{
			get { return _assetName; }
			set { _assetName = value; }
		}

		private int _assetInitialCount = 0;

		public bool IsConditionComplete(Level level)
		{

			if (_assetInitialCount == 0)
				_assetInitialCount = level.Actors.Where(a => a.Texture.Name == PickupAssetName).Count();

			var actorsLeft = level.Actors.Where(a => a.Texture.Name == PickupAssetName).Count();

			return actorsLeft < _assetInitialCount;
		}

	}
}
