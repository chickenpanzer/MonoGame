using MonoGame.Core;
using System;
using System.Collections.ObjectModel;

namespace ToolKit
{
	public class VictoryConditionViewModel : ViewModelBase
	{
		private RelayCommand<LevelVictoryConditionsVictoryCondition> _deleteVictoryConditionCommand;
		private ObservableCollection<LevelVictoryConditionsVictoryCondition> _victoryConditions;
		private RelayCommand _addVictoryConditionCommand;
		private RelayCommand<LevelVictoryConditionsVictoryCondition> _setVictoryConditionAssetCommand;

		public ObservableCollection<LevelVictoryConditionsVictoryCondition> VictoryConditions
		{
			get
			{
				return _victoryConditions;
			}

			set
			{
				_victoryConditions = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand<LevelVictoryConditionsVictoryCondition> DeleteVictoryConditionCommand
		{
			get => _deleteVictoryConditionCommand = _deleteVictoryConditionCommand ?? new RelayCommand<LevelVictoryConditionsVictoryCondition>(DeleteVictoryCondition);
			set => _deleteVictoryConditionCommand = value;
		}

		private void DeleteVictoryCondition(LevelVictoryConditionsVictoryCondition obj)
		{
			var vc = obj as LevelVictoryConditionsVictoryCondition;

			VictoryConditions.Remove(vc);
		}

		public RelayCommand AddVictoryConditionCommand
		{
			get => _addVictoryConditionCommand = _addVictoryConditionCommand ?? new RelayCommand(AddVictoryCondition);
			set => _addVictoryConditionCommand = value;
		}

		private void AddVictoryCondition(object obj)
		{
			VictoryConditions.Add(new LevelVictoryConditionsVictoryCondition() { @class = string.Empty, assetName = string.Empty });
		}

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
