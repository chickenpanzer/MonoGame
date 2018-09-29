using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit
{
	public class MonsterViewModel : ViewModelBase
	{
		private string _monsterHealthOption;
		private string _monsterAttackOption;
		private string _monsterDefenseOption;
		private string _monsterScoreOption;
		private string _selectedMonsterClassOption;
		private string _selectedMonster;

		public string MonsterHealthOption
		{
			get => _monsterHealthOption;
			set
			{
				_monsterHealthOption = value;
				RaisePropertyChanged();
			}
		}

		public string MonsterAttackOption
		{
			get => _monsterAttackOption;
			set
			{
				_monsterAttackOption = value;
				RaisePropertyChanged();
			}
		}

		public string MonsterDefenseOption
		{
			get => _monsterDefenseOption;
			set
			{
				_monsterDefenseOption = value;
				RaisePropertyChanged();
			}
		}

		public string MonsterScoreOption
		{
			get => _monsterScoreOption;
			set
			{
				_monsterScoreOption = value;
				RaisePropertyChanged();
			}
		}

		public string SelectedMonsterClassOption
		{
			get => _selectedMonsterClassOption;
			set
			{
				_selectedMonsterClassOption = value;
				RaisePropertyChanged();
			}
		}

		public string SelectedMonster
		{
			get => _selectedMonster;
			set
			{
				_selectedMonster = value;
				RaisePropertyChanged();
			}
		}

		public void SelectMonster(object obj)
		{
			SelectedMonster = (string)obj;
		}


	}
}
