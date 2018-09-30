using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ToolKit
{
	public class ItemsViewModel : ViewModelBase
	{
		private string _healthOption;
		private string _attackOption;
		private string _defenseOption;
		private string _scoreOption;
		private string _lightScaleOption;
		private string _selectedItem;
		private string _selectedSoundOption;

		public string HealthOption
		{
			get => _healthOption;
			set
			{
				_healthOption = value;
				RaisePropertyChanged();
			}
		}

		public string AttackOption
		{
			get => _attackOption;
			set
			{
				_attackOption = value;
				RaisePropertyChanged();
			}
		}

		public string DefenseOption
		{
			get => _defenseOption;
			set
			{
				_defenseOption = value;
				RaisePropertyChanged();
			}
		}

		public string ScoreOption
		{
			get => _scoreOption;
			set
			{
				_scoreOption = value;
				RaisePropertyChanged();
			}
		}

		public string LightScaleOption
		{
			get => _lightScaleOption;
			set
			{
				_lightScaleOption = value;
				RaisePropertyChanged();
			}
		}

		public string SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;
				RaisePropertyChanged();
				RaisePropertyChanged("SelectedItemImage");
			}
		}

		public BitmapImage SelectedItemImage
		{
			get
			{
				BitmapImage img = null;
				GenericRessource.GraphicRessources["Items"].TryGetValue(SelectedItem, out img);
				return img;
			}
		}

		public string SelectedSoundOption
		{
			get => _selectedSoundOption;
			set
			{
				_selectedSoundOption = value;
				RaisePropertyChanged();
			}
		}

		public void SelectItem(object obj)
		{
			SelectedItem = (string)obj;
		}

	}
}
