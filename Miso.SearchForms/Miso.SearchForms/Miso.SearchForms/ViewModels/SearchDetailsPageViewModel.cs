using Miso.SearchForms.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Miso.SearchForms.ViewModels
{
	public class SearchDetailsPageViewModel : BindableBase, INavigationAware
	{
		public SearchDetailsPageViewModel()
		{

		}

		/// <summary>
		/// アイテムを示すパラメーターキー
		/// </summary>
		public const string SearchItemParameterKey = "c";

		private SearchItem _item = null;

		public SearchItem Item
		{
			get { return this._item; }
			set { this.SetProperty(ref _item, value); }
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		/// <summary>
		/// ここに来たとき
		/// </summary>
		/// <param name="parameters"></param>
		public void OnNavigatedTo(NavigationParameters parameters)
		{
			//パラメーターにアイテムが設定している前提で、それを表示する
			SearchItem item = parameters[SearchDetailsPageViewModel.SearchItemParameterKey] as SearchItem;
			Debug.Assert(item != null);

			this.Item = item;
		}
	}
}
