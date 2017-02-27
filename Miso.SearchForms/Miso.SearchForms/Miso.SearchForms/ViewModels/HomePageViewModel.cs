using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miso.SearchForms.ViewModels
{
	public class HomePageViewModel : BindableBase
	{
		public HomePageViewModel()
		{

		}

		private string _title = "title desu";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}
	}
}
