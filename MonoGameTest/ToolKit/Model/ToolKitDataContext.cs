﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;

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
			_rows = new ObservableCollection<ObservableCollection<LevelTilesTile>>();
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
			set
			{
				_isWalkableOption = value;
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

			if (!string.IsNullOrEmpty(SelectedAsset))
				layer.assetName = SelectedAsset;


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
				if (!string.IsNullOrEmpty(SelectedItem))
				{
					actor.attackValue = AttackOption;
					actor.defenseValue = DefenseOption;
					actor.scoreValue = ScoreOption;
					actor.healthValue = HealthOption;
					actor.lightScale = LightScaleOption;
				}
				else
				{
					actor.attackValue = null;
					actor.defenseValue = null;
					actor.scoreValue = null;
					actor.healthValue = null;
					actor.lightScale = null;
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

		private ObservableCollection<ObservableCollection<LevelTilesTile>> _rows = null;
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
		private RelayCommand _generateLevelCommand;
		private Level _level;

		internal void LoadXMLTemplate(string XMLFileName)
		{
			Level = XMLHelper.ReadXMLToObject<Level>(XMLFileName);

			//Explode level in layers and rows
			Rows = ExplodeLevel(Level);

		}

		private ObservableCollection<ObservableCollection<LevelTilesTile>> ExplodeLevel(Level level)
		{
			var rows = new ObservableCollection<ObservableCollection<LevelTilesTile>>();
			var row = new ObservableCollection<LevelTilesTile>();
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