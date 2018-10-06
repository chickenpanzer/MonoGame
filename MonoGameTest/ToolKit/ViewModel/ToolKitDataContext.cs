using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace ToolKit
{
	public class ToolKitDataContext : ViewModelBase
	{
		
		public ToolKitDataContext()
		{
			_rows = new ObservableCollection<ObservableCollection<LevelTilesTile>>();

			// TODO: make sound loading dynamic
			_soundListOption = new ObservableCollection<string>() { "NONE", "pickup", "apple_bite" };
			_monsterClassOption = new ObservableCollection<string>() { "Monster", "Amobea" };
			_monsterMoverClassOption = new ObservableCollection<string>() { "RandomMover" };
			_victoryConditionClassOption = new ObservableCollection<string>() {"ExitVictoryCondition","PickAllVictoryCondition", "PickOneVictoryCondition" };

			//Items and Monster datacontext
			ItemsViewModel = new ItemsViewModel();
			MonsterViewModel = new MonsterViewModel();
			VictoryConditionViewModel = new VictoryConditionViewModel();
		}

		public ItemsViewModel ItemsViewModel { get; set; }

		public MonsterViewModel MonsterViewModel { get; set; }
		public VictoryConditionViewModel VictoryConditionViewModel
		{
			get
			{
				return _victoryConditionViewModel;
			}

			private set
			{
				_victoryConditionViewModel = value;
				RaisePropertyChanged();
			}
		}

		private bool[] _modeArray = new bool[] { true, false, false };
		public bool[] ModeArray
		{
			get { return _modeArray; }
		}

		enum EditModes
		{
			FloorMode = 0,
			ItemMode,
			MonsterMode
		}

		private EditModes EditMode()
		{
			if (ModeArray[0])
				return EditModes.FloorMode;

			if (ModeArray[1])
				return EditModes.ItemMode;

			if (ModeArray[2])
				return EditModes.MonsterMode;

			return EditModes.FloorMode;
		}

		public int SelectedMode
		{
			get { return Array.IndexOf(_modeArray, true); }
		}

		public Level Level
		{
			get { return _level; }
			set
			{
				_level = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> SoundListOption
		{
			get => _soundListOption;
			set
			{
				_soundListOption = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> MonsterClassOption
		{
			get => _monsterClassOption;
			set
			{
				_monsterClassOption = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> MonsterMoverClassOption
		{
			get => _monsterMoverClassOption;
			set
			{
				_monsterMoverClassOption = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<string> VictoryConditionClassOption
		{
			get => _victoryConditionClassOption;
			set
			{
				_victoryConditionClassOption = value;
				RaisePropertyChanged();
			}
		}



		public ObservableCollection<ObservableCollection<LevelTilesTile>> Rows
		{
			get => _rows;
			set
			{
				_rows = value;
				RaisePropertyChanged();
			}
		}

		public string SelectedAsset
		{
			get => _selectedAsset;
			set
			{
				_selectedAsset = value;
				RaisePropertyChanged();
				RaisePropertyChanged("SelectedAssetImage");
			}
		}

		public BitmapImage SelectedAssetImage
		{
			get
			{
				BitmapImage img = null;
				GenericRessource.GraphicRessources["Floor"].TryGetValue(SelectedAsset, out img);
				return img;
			}
		}

		private int _rowsOption;

		public int RowsOption
		{
			get { return _rowsOption; }
			set { _rowsOption = value; RaisePropertyChanged(); }
		}

		private int _columnsOption;

		public int ColumnsOption
		{
			get { return _columnsOption; }
			set { _columnsOption = value; RaisePropertyChanged(); }
		}

		private string _levelNameOption;

		public string LevelNameOption
		{
			get { return _levelNameOption; }
			set { _levelNameOption = value; RaisePropertyChanged(); }
		}

		public RelayCommand SelectAssetCommand
		{
			get => _selectAssetCommand = _selectAssetCommand ?? new RelayCommand(SelectAsset, null);
			set => _selectAssetCommand = value;
		}

		public RelayCommand SelectItemCommand
		{
			get => _selectItemCommand = _selectItemCommand ?? new RelayCommand(ItemsViewModel.SelectItem, null);
			set => _selectItemCommand = value;
		}

		public RelayCommand SelectMonsterCommand
		{
			get => _selectMonsterCommand = _selectMonsterCommand ?? new RelayCommand(MonsterViewModel.SelectMonster, null);
			set => _selectMonsterCommand = value;
		}

		public RelayCommand ApplyAssetCommand
		{
			get => _applyAssetCommand = _applyAssetCommand ?? new RelayCommand(ApplyAsset, null);
			set => _applyAssetCommand = value;
		}

		public RelayCommand WriteXmlCommand
		{
			get => _writeXmlCommand = _writeXmlCommand ?? new RelayCommand(WriteXml, CanWriteXml);
			set => _writeXmlCommand = value;
		}

		public RelayCommand ReadXmlCommand
		{
			get => _readXmlCommand = _readXmlCommand ?? new RelayCommand(ReadXml, null);
			set => _readXmlCommand = value;
		}

		public RelayCommand GetTileInfoCommand
		{
			get => _getTileInfoCommand = _getTileInfoCommand ?? new RelayCommand(GetTileInfo, null);
			set => _getTileInfoCommand = value;
		}

		public RelayCommand GenerateLevelCommand
		{
			get => _generateLevelCommand = _generateLevelCommand ?? new RelayCommand(GenerateLevel, CanGenerate);
			set => _generateLevelCommand = value;
		}

		private bool CanGenerate(object arg)
		{
			return (!string.IsNullOrEmpty(LevelNameOption) && RowsOption > 0 && ColumnsOption > 0);
		}

		//Generate new level
		private void GenerateLevel(object obj)
		{
			var generator = new LevelGenerator(RowsOption, ColumnsOption);

			Level = generator.GenerateLevel(LevelNameOption, SelectedAsset);
			Rows = ExplodeLevel(Level);
		}

		public LevelTilesTile SelectedTile
		{
			get => _selectedTile;
			set
			{
				_selectedTile = value;
				RaisePropertyChanged();
			}
		}

		public bool IsWalkableOption
		{
			get => _isWalkableOption;
			set
			{
				_isWalkableOption = value;
				RaisePropertyChanged();
			}
		}

		private void WriteXml(object obj)
		{
			var writer = new LevelWriter();
			var lvl = writer.AddRessourcesToLevel(Level);

			//Prompt for file location
			var win = new SaveFileDialog();
			win.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			win.Filter = "Fichiers level (*.xml)|*.xml";

			win.ShowDialog();

			var fullPath = win.FileName;

			if (!string.IsNullOrEmpty(fullPath))
			{
				XMLHelper.WriteObjectToXML<Level>(fullPath, lvl);
			}
		}

		private void ReadXml(object obj)
		{

			var win = new OpenFileDialog();
			win.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			win.Filter = "Fichiers level (*.xml)|*.xml";

			win.ShowDialog();

			var fullPath = win.FileName;

			if (!string.IsNullOrEmpty(fullPath))
			{
				this.LoadXMLTemplate(fullPath);
			}
		}

		private bool CanWriteXml(object arg)
		{
			return Level != null;
		}


		/// <summary>
		/// Retrieve data from current tile for duplication according to current selected mode
		/// </summary>
		/// <param name="obj"></param>
		private void GetTileInfo(object obj)
		{
			var tile = obj as LevelTilesTile;


			if (EditMode() == EditModes.FloorMode)
			{
				bool.TryParse(tile.isWalkable, out bool walkable);
				string asset = tile.Layer[0].assetName;

				//Set selected asset and data
				SelectedAsset = asset;
				IsWalkableOption = walkable;
			}

			if (tile.Actor != null)
			{
				var actor = tile.Actor[0];

				if (actor.@class == "Pickup")
				{
					ItemsViewModel.HealthOption = actor.healthValue;
					ItemsViewModel.ScoreOption = actor.scoreValue;
					ItemsViewModel.AttackOption = actor.attackValue;
					ItemsViewModel.DefenseOption = actor.defenseValue;
					ItemsViewModel.SelectedItem = actor.assetName;
					ItemsViewModel.LightScaleOption = actor.lightScale;
					ItemsViewModel.SelectedSoundOption = actor.pickupSound;
				}
				else
				{
					MonsterViewModel.MonsterHealthOption = actor.healthValue;
					MonsterViewModel.MonsterScoreOption = actor.scoreValue;
					MonsterViewModel.MonsterAttackOption = actor.attackValue;
					MonsterViewModel.MonsterDefenseOption = actor.defenseValue;
					MonsterViewModel.SelectedMonster = actor.assetName;
				}
			}
		}
		/// <summary>
		/// On click, apply assets to corresponding tile
		/// </summary>
		/// <param name="obj"></param>
		private void ApplyAsset(object obj)
		{
			var tile = obj as LevelTilesTile;
			var layer = tile.Layer[0];

			//Floor mode
			if (EditMode() == EditModes.FloorMode)
			{
				if (!string.IsNullOrEmpty(SelectedAsset))
					layer.assetName = SelectedAsset;

				tile.isWalkable = IsWalkableOption.ToString();
			}

			LevelTilesTileActor actor = tile.Actor[0];

			//Items Mode
			if (EditMode() == EditModes.ItemMode)
			{
				if (!string.IsNullOrEmpty(ItemsViewModel.SelectedItem))
				{
					actor.assetName = ItemsViewModel.SelectedItem;
					actor.@class = "Pickup";
					actor.attackValue = ItemsViewModel.AttackOption;
					actor.defenseValue = ItemsViewModel.DefenseOption;
					actor.scoreValue = ItemsViewModel.ScoreOption;
					actor.healthValue = ItemsViewModel.HealthOption;
					actor.lightScale = ItemsViewModel.LightScaleOption;
					actor.pickupSound = ItemsViewModel.SelectedSoundOption;
				}
				else
				{
					ResetActor(actor);
				}
			}

			//Monster Mode
			if (EditMode() == EditModes.MonsterMode)
			{
				if (!string.IsNullOrEmpty(MonsterViewModel.SelectedMonster))
				{
					actor.assetName = MonsterViewModel.SelectedMonster;
					actor.@class = MonsterViewModel.SelectedMonsterClassOption;
					actor.attackValue = MonsterViewModel.MonsterAttackOption;
					actor.defenseValue = MonsterViewModel.MonsterDefenseOption;
					actor.scoreValue = MonsterViewModel.MonsterScoreOption;
					actor.healthValue = MonsterViewModel.MonsterHealthOption;
					if (!string.IsNullOrEmpty(MonsterViewModel.SelectedMonsterMoverClassOption))
					{
						var mover = new LevelTilesTileActorMover() { @class = MonsterViewModel.SelectedMonsterMoverClassOption };
						mover.moveSpeed = "32";
						mover.interval = MonsterViewModel.MonsterMoveIntervalOption;
						actor.Mover = mover;
					}

				}
				else
				{
					ResetActor(actor);
				}

			}

		}

		private static void ResetActor(LevelTilesTileActor actor)
		{
			actor.@class = null;
			actor.attackValue = null;
			actor.defenseValue = null;
			actor.scoreValue = null;
			actor.healthValue = null;
			actor.lightScale = null;
			actor.pickupSound = null;
			actor.assetName = null;
			actor.Mover = null;
		}

		private void SelectAsset(object obj)
		{
			SelectedAsset = (string)obj;
		}

		private ObservableCollection<ObservableCollection<LevelTilesTile>> _rows = null;
		private string _selectedAsset;
		
		private LevelTilesTile _selectedTile;
		private bool _isWalkableOption;

		private RelayCommand _selectAssetCommand = null;
		private RelayCommand _applyAssetCommand = null;
		private RelayCommand _writeXmlCommand = null;
		private RelayCommand _getTileInfoCommand = null;
		private RelayCommand _selectItemCommand;
		private RelayCommand _generateLevelCommand;
		private RelayCommand _readXmlCommand;
		private RelayCommand _selectMonsterCommand;

		private Level _level;
		private ObservableCollection<string> _soundListOption;
		private ObservableCollection<string> _monsterClassOption;
		private ObservableCollection<string> _monsterMoverClassOption;
		private ObservableCollection<string> _victoryConditionClassOption;
		private VictoryConditionViewModel _victoryConditionViewModel;

		internal void LoadXMLTemplate(string XMLFileName)
		{
			Level = XMLHelper.ReadXMLToObject<Level>(XMLFileName);

			//Explode level in layers and rows
			Rows = ExplodeLevel(Level);

			VictoryConditionViewModel = new VictoryConditionViewModel(Level);

		}

		private ObservableCollection<ObservableCollection<LevelTilesTile>> ExplodeLevel(Level level)
		{
			var rows = new ObservableCollection<ObservableCollection<LevelTilesTile>>();
			var row = new ObservableCollection<LevelTilesTile>();
			string prevRow = "0";

			//Order tiles by column then row
			var tiles = level.Tiles.OrderBy(t => int.Parse(t.posY)).ThenBy(t => int.Parse(t.posX));

			foreach (var tile in tiles)
			{
				if (tile.posY == prevRow)
				{
					row.Add(tile);
					prevRow = tile.posY;
				}
				else
				{
					rows.Add(row);
					row = new ObservableCollection<LevelTilesTile>();
					row.Add(tile);
					prevRow = tile.posY;
				}
			}

			//Final row
			rows.Add(row);

			return rows;
		}

	}
}
