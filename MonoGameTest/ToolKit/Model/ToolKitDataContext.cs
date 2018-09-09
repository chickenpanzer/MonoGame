using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace ToolKit
{
	public class ToolKitDataContext : INotifyPropertyChanged
	{
		//InotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ToolKitDataContext()
		{
			_rows = new List<List<LevelTilesTile>>();
		}

		public Level Level
		{
			get;
			set;
		}
		public List<List<LevelTilesTile>> Rows { get => _rows; set => _rows = value; }

		public string SelectedAsset
		{
			get => _selectedAsset;
			set
			{
				_selectedAsset = value;
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
			}
		}

		public RelayCommand SelectAssetCommand
		{
			get => _selectAssetCommand = _selectAssetCommand ?? new RelayCommand(SelectAsset, null);
			set => _selectAssetCommand = value;
		}

		public RelayCommand SelectItemCommand
		{
			get => _selectItemCommand = _selectItemCommand ?? new RelayCommand(SelectItem, null);
			set => _selectItemCommand = value;
		}

		public RelayCommand ApplyAssetCommand
		{
			get => _applyAssetCommand = _applyAssetCommand ?? new RelayCommand(ApplyAsset, null);
			set => _applyAssetCommand = value;
		}

		public RelayCommand WriteXmlCommand
		{
			get => _writeXmlCommand = _writeXmlCommand ?? new RelayCommand(WriteXml, null);
			set => _writeXmlCommand = value;
		}

		public RelayCommand GetTileInfoCommand
		{
			get => _getTileInfoCommand = _getTileInfoCommand ?? new RelayCommand(GetTileInfo, null);
			set => _getTileInfoCommand = value; }


		/// <summary>
		/// Retrieve data from current tile for duplication
		/// </summary>
		/// <param name="obj"></param>
		private void GetTileInfo(object obj)
		{
			var tile = obj as LevelTilesTile;

			bool.TryParse(tile.isWalkable, out bool walkable);
			string asset = tile.Layer[0].assetName;

			//Set selected asset and data
			SelectedAsset = asset;
			IsWalkableOption = walkable;
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
			set { _isWalkableOption = value;
				RaisePropertyChanged();
			}
		}

		

		private void WriteXml(object obj)
		{
			XMLHelper.WriteObjectToXML<Level>("testWrite.xml", Level);
		}

		private void ApplyAsset(object obj)
		{
			var tile = obj as LevelTilesTile;
			var layer = tile.Layer[0];

			if(!string.IsNullOrEmpty(SelectedAsset))
				layer.assetName = SelectedAsset;

			if (!string.IsNullOrEmpty(SelectedItem))
			{
				LevelTilesTileActor actor = null;

				if (tile.Actor == null)
				{
					tile.Actor = tile.Actor.Redim<LevelTilesTileActor>(true);
					actor = new LevelTilesTileActor();
					actor.@class = "Pickup";
					actor.assetName = SelectedItem;
					actor.attackValue = AttackOption;
					actor.defenseValue = DefenseOption;
					actor.scoreValue = ScoreOption;
					actor.healthValue = HealthOption;
					actor.lightScale = LightScaleOption;
					tile.Actor[0] = actor;

					RaisePropertyChanged("Level");

				}
				else
				{
					actor = tile.Actor[0];
					actor.assetName = SelectedItem;
					actor.attackValue = AttackOption;
					actor.defenseValue = DefenseOption;
					actor.scoreValue = ScoreOption;
					actor.healthValue = HealthOption;
					actor.lightScale = LightScaleOption;
				}
			}
				
			tile.isWalkable = IsWalkableOption.ToString();

		}

		private void SelectAsset(object obj)
		{
			SelectedAsset = (string)obj;
		}

		private void SelectItem(object obj)
		{
			SelectedItem = (string)obj;
		}

		private List<List<LevelTilesTile>> _rows = null;
		private string _selectedAsset;
		private string _selectedItem;
		private LevelTilesTile _selectedTile;
		private bool _isWalkableOption;

		private string _healthOption;
		private string _attackOption;
		private string _defenseOption;
		private string _scoreOption;
		private string _lightScaleOption;

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



		private RelayCommand _selectAssetCommand = null;
		private RelayCommand _applyAssetCommand = null;
		private RelayCommand _writeXmlCommand = null;
		private RelayCommand _getTileInfoCommand = null;
		private RelayCommand _selectItemCommand;

		internal void LoadXMLTemplate(string XMLFileName)
		{
			Level = XMLHelper.ReadXMLToObject<Level>(XMLFileName);

			//Explode level in layers and rows
			ExplodeLevel(Level, Rows);

		}

		private void ExplodeLevel(Level level, List<List<LevelTilesTile>> rows)
		{

			var row = new List<LevelTilesTile>();
			string prevRow = "0";

			//Order tiles by column then row
			var tiles = level.Tiles.OrderBy(t => t.posY).ThenBy(t => t.posX);

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
					row = new List<LevelTilesTile>();
					row.Add(tile);
					prevRow = tile.posY;
				}
			}

			//Final row
			rows.Add(row);
		}

	}
}
