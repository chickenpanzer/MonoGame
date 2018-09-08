using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolKit
{

	public class RelayCommand<T> : ICommand
	{
		private Action<T> _execute;
		private Func<T, bool> _canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			this._execute = execute;
			this._canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this._canExecute == null || this._canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			this._execute((T)parameter);
		}
	}
}
