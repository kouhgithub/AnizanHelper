﻿using System;
using System.Windows.Input;

namespace AnizanHelper.ViewModels
{
	public class DelegateCommand : ICommand
	{
		public Action<object> ExecuteHandler { get; set; }
		public Func<object, bool> CanExecuteHandler { get; set; }

		#region ICommandメンバ
		public bool CanExecute(object parameter)
		{
			var d = this.CanExecuteHandler;
			return d == null ? true : d(parameter);
		}

		public void Execute(object parameter)
		{
			var d = this.ExecuteHandler;
			if (d != null)
			{
				d(parameter);
			}
		}

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			var d = CanExecuteChanged;
			if (d != null)
			{
				d(this, null);
			}
		}

		#endregion
	}
}