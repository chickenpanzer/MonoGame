using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;

namespace ToolKit
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new ToolKitDataContext(); 
		}

		private void SelectedAsset_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var source = sender as TextBlock;

				DragDropEffects effects;

				DataObject obj = new DataObject();

				obj.SetData(typeof(string), source.Text);

				effects = DragDrop.DoDragDrop(SelectedAsset, obj, DragDropEffects.Copy | DragDropEffects.Move);

			}
		}

		private void TextBox_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(string)))
			{
				e.Effects = DragDropEffects.Copy;
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void TextBox_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(string)))
			{
				e.Effects = DragDropEffects.Copy;
				string uri = (string)e.Data.GetData(typeof(string));
				// Utiliser uri comme vous le souhaitez

			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
		}

		private void SelectedItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var source = sender as TextBlock;

				DragDropEffects effects;

				DataObject obj = new DataObject();

				obj.SetData(typeof(string), source.Text);

				effects = DragDrop.DoDragDrop(SelectedItem, obj, DragDropEffects.Copy | DragDropEffects.Move);

			}
		}
	}
}
