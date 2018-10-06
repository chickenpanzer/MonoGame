using MonoGame.Core;
using System.Collections.ObjectModel;

namespace ToolKit
{
	public class VictoryConditionViewModel : ViewModelBase
	{

		public ObservableCollection<LevelVictoryConditionsVictoryCondition> VictoryConditions { get; set; }

		public VictoryConditionViewModel()
		{
			VictoryConditions = new ObservableCollection<LevelVictoryConditionsVictoryCondition>();
		}

		public VictoryConditionViewModel(Level level)
		{
			VictoryConditions = new ObservableCollection<LevelVictoryConditionsVictoryCondition>();

			if (level.VictoryConditions != null && level.VictoryConditions.Length > 0)
			{
				VictoryConditions = new ObservableCollection<LevelVictoryConditionsVictoryCondition>(level.VictoryConditions);
			}

		}
	}
}
