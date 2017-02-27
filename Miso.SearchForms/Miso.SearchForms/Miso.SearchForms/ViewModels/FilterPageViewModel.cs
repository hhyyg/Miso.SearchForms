using Microsoft.Azure.Search.Models;
using Miso.SearchForms.Common;
using Miso.SearchForms.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Miso.SearchForms.ViewModels
{
	/// <summary>
	/// Filter option dialog for SearchPage
	/// </summary>
	public class FilterPageViewModel : BindableBase, INavigationAware
	{
		public FilterPageViewModel(
			INavigationService navigationService)
		{
			_navigationService = navigationService;
			this.ApplyFilterCommand = new DelegateCommand(() => this.ApplyFilter().FireAndForget());
			this.CancelCommand = new DelegateCommand(() => this.Cancel().FireAndForget());
			SetSearchFilter(SearchFilter.Default);
		}

		private INavigationService _navigationService { get; }
		public DelegateCommand ApplyFilterCommand { get; }
		public DelegateCommand CancelCommand { get; }

		private int _bedsMin = 0;
		public int BedsMin
		{
			get { return _bedsMin; }
			set { SetProperty(ref _bedsMin, value); }
		}
		private int _bedsMax = 0;
		public int BedsMax
		{
			get { return _bedsMax; }
			set { SetProperty(ref _bedsMax, value); }
		}

		private bool _statusSale = true;
		public bool StatusSale
		{
			get { return _statusSale; }
			set { SetProperty(ref _statusSale, value); }
		}
		private bool _statusPending = true;
		public bool StatusPending
		{
			get { return _statusPending; }
			set { SetProperty(ref _statusPending, value); }
		}

		private bool _statusSold = true;
		public bool StatusSold
		{
			get { return _statusSold; }
			set { SetProperty(ref _statusSold, value); }
		}

		private async Task ApplyFilter()
		{
			var parameters = new NavigationParameters();
			parameters.Add(SearchPageViewModel.SearchFilterParameterKey, GetSearchFilter());

			await _navigationService.GoBackAsync(parameters: parameters, useModalNavigation: true);
		}
		
		private async Task Cancel()
		{
			await _navigationService.GoBackAsync(parameters: null, useModalNavigation: true);
		}

		private SearchFilter GetSearchFilter()
		{
			var filter = new SearchFilter()
			{
				BedsMin = this.BedsMin,
				BedsMax = this._bedsMax,
				IncludeStatusSale = this.StatusSale,
				IncludeStatusPending = this.StatusPending,
				IncludeStatusSold = this.StatusSold
			};
			
			return filter;
		}

		private void SetSearchFilter(SearchFilter filter)
		{
			this.BedsMin = filter.BedsMin;
			this.BedsMax = filter.BedsMax;
			this.StatusSale = filter.IncludeStatusSale;
			this.StatusPending = filter.IncludeStatusPending;
			this.StatusSold = filter.IncludeStatusSold;
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			SearchFilter filter = parameters.GetValueOrDefault<SearchFilter>(SearchPageViewModel.SearchFilterParameterKey);
			SetSearchFilter(filter ?? SearchFilter.Default);
		}
	}
}
