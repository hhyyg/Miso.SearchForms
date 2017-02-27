using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Miso.SearchForms.Common;
using Miso.SearchForms.Models;
using Miso.SearchForms.Models.SearchFacets;
using Miso.SearchForms.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms.ViewModels
{
	public class SearchPageViewModel : BindableBase, INavigationAware
	{
		public SearchPageViewModel(
			IPageDialogService pageDialogService,
			INavigationService navigationService,
			IAzureSearchClient azureSearchClient)
		{
			this._pageDialogService = pageDialogService;
			this._navigationService = navigationService;
			this._azureSearchClient = azureSearchClient;
			this.ItemList = new ObservableCollection<SearchItem>();
			this.OnListViewItemSelectedCommand = new DelegateCommand<SearchItem>((item) => this.OnListViewItemSelected(item).FireAndForget());
			this.OpenFilterPageCommand = new DelegateCommand(() => this.OpenFilterPage().FireAndForget());
			this.TypeSelectionChangedCommand = new DelegateCommand<int?>((val) => this.TypeSelectionChanged(val.Value).FireAndForget());

			_filter = SearchFilter.Default;
		}
		
		/// <summary>
		/// Search Term Key
		/// </summary>
		public const string SearchTextParameterKey = "p";
		/// <summary>
		/// Search Filter Key
		/// </summary>
		public const string SearchFilterParameterKey = "f";

		public const int SearchFacet_Price_Interval = 1000000;
		
		private IPageDialogService _pageDialogService { get; }
		private INavigationService _navigationService { get; }
		private IAzureSearchClient _azureSearchClient { get; }

		private SearchFilter _filter { get; set; }

		private HomeType _facetHomeType = HomeType.All;

		public ObservableCollection<SearchItem> ItemList { get; private set; }
		private string _searchText = string.Empty;
		public string SearchText
		{
			get { return _searchText; }
			set { SetProperty(ref _searchText, value); }
		}

		private bool _isBusy = false;
		public bool IsBusy
		{
			get { return this._isBusy; }
			set { this.SetProperty(ref this._isBusy, value); }
		}

		private int _segmentTypeAllCount = 0;
		/// <summary>
		/// タイプ 全て
		/// </summary>
		public ItemCountPair SegmentTypeAll => new ItemCountPair(nameof(HomeType.All), _segmentTypeAllCount);
		
		private int _segmentTypeApartmentCount = 0;
		/// <summary>
		/// タイプ アパート
		/// </summary>
		/// </summary>
		public ItemCountPair SegmentTypeApartment => new ItemCountPair(nameof(HomeType.Apartment), _segmentTypeApartmentCount);

		private int _segmentTypeHouseCount = 0;
		/// <summary>
		/// タイプ 家
		/// </summary>
		public ItemCountPair SegmentTypeHouse => new ItemCountPair(nameof(HomeType.House), _segmentTypeHouseCount);
		
		private void ChangeFacetResultCount(HomeType homeType, int newCount)
		{
			switch(homeType)
			{
				case HomeType.All:
					SetProperty(ref _segmentTypeAllCount, newCount, nameof(SegmentTypeAll));
					break;
				case HomeType.Apartment:
					SetProperty(ref _segmentTypeApartmentCount, newCount, nameof(SegmentTypeApartment));
					break;
				case HomeType.House:
					SetProperty(ref _segmentTypeHouseCount, newCount, nameof(SegmentTypeHouse));
					break;
			}
		}

		/// <summary>
		/// タイプが変更されたとき
		/// </summary>
		public DelegateCommand<int?> TypeSelectionChangedCommand { get; }

		/// <summary>
		/// 検索結果 選択
		/// </summary>
		public DelegateCommand<SearchItem> OnListViewItemSelectedCommand { get; }
		/// <summary>
		/// フィルター画面 表示
		/// </summary>
		public DelegateCommand OpenFilterPageCommand { get; }

		/// <summary>
		/// Facetをリセットします
		/// </summary>
		private void ResetFacet()
		{
			this._facetHomeType = HomeType.All;
		}

		/// <summary>
		/// タイプが変更されたとき
		/// </summary>
		/// <param name="newSelectedIndex"></param>
		/// <returns></returns>
		private async Task TypeSelectionChanged(int newSelectedIndex)
		{
			//どのタイプを選択したのか取得する
			if (Enum.TryParse<HomeType>(newSelectedIndex.ToString(), out this._facetHomeType) == false)
			{
				ResetFacet();
			}

			//検索（Facetつきで検索。このとき、画面のタイプの更新は行わない）
			await Search(this.SearchText, this._filter, updateHomeTypeFacetStatus: false);
		}

		/// <summary>
		/// フィルター画面 表示
		/// </summary>
		/// <returns></returns>
		private async Task OpenFilterPage()
		{
			var parameters = new NavigationParameters();
			parameters.Add(SearchPageViewModel.SearchFilterParameterKey, this._filter);

			await this._navigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(FilterPage)}",
				parameters: parameters,
				useModalNavigation: true,
				animated: true);
		}

		/// <summary>
		/// 検索結果 選択時
		/// </summary>
		/// <param name="searchItem"></param>
		/// <returns></returns>
		private async Task OnListViewItemSelected(SearchItem searchItem)
		{
			//結果を選択したら、詳細画面に遷移
			if (searchItem != null)
			{
				var parameter = new NavigationParameters();
				parameter.Add(SearchDetailsPageViewModel.SearchItemParameterKey, searchItem); 

				await this._navigationService.NavigateAsync(nameof(SearchDetailsPage), parameter);
			}
		}

		/// <summary>
		/// using で IsBusy の切り替えができるようにするためのメソッド。
		/// </summary>
		IDisposable BeginBusy()
		{
			this.IsBusy = true;
			return Disposable.Create(() => this.IsBusy = false);
		}

		/// <summary>
		/// 検索します
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="filter"></param>
		/// <param name="updateHomeTypeFacetStatus"></param>
		/// <returns></returns>
		private async Task Search(
			string searchText, 
			SearchFilter filter,
			bool updateHomeTypeFacetStatus = false)
		{
			Debug.Assert(string.IsNullOrEmpty(searchText) == false);
			if (filter == null)
				filter = SearchFilter.Default;

			ItemList.Clear();
			DocumentSearchResult<SearchItem> searchResults;

			using (BeginBusy())
			{
				searchResults = await SearchAsync(searchText, filter, this._facetHomeType);
			}
			
			//facetを出力
			if (updateHomeTypeFacetStatus)
			{
				SetFacetResults(searchResults);
			}

			//Itemを出力
			foreach (SearchResult<SearchItem> result in searchResults.Results)
			{
				ItemList.Add(result.Document);
			}

			if (searchResults.Results.Any() == false)
			{
				ResetFacet();
				await this._pageDialogService.DisplayAlertAsync(
					title: "NotFound",
					message: $"Search:{searchText} not found",
					cancelButton: "OK");
				return;
			}
		}

		/// <summary>
		/// 検索結果のうちFacetを設定します
		/// </summary>
		/// <param name="searchResults"></param>
		private void SetFacetResults(DocumentSearchResult<SearchItem> searchResults)
		{
			//タイプの情報を出力
			IList<FacetResult> typeFacets;
			if (searchResults.Facets.TryGetValue("type", out typeFacets))
			{
				foreach (var facet in typeFacets)
				{
					var facetType = (HomeType)Enum.Parse(typeof(HomeType), facet.Value.ToString());
					ChangeFacetResultCount(facetType, facet.Count);
				}
			}

			ChangeFacetResultCount(HomeType.All, searchResults.Count);
		}
		
		private void ChangeFacetResultCount(HomeType type, long? count)
		{
			ChangeFacetResultCount(type, (int?)count ?? 0);
		}

		/// <summary>
		/// 検索を行います
		/// </summary>
		/// <param name="searchText">検索するキーワード</param>
		/// <param name="filter">検索条件</param>
		/// <param name="homeType">Homeタイプ</param>
		/// <returns></returns>
		private async Task<DocumentSearchResult<SearchItem>> SearchAsync(
			string searchText, 
			SearchFilter filter,
			HomeType homeType)
		{
			var parameter = new SearchParameters()
			{
				SearchMode = SearchMode.Any,
				Top = 10,
				Facets = new string[] { "type" },
				IncludeTotalResultCount = true
			};

			if (filter != null)
			{
				//homeTypeはFacetなので、検索条件（SearchFilter.cs）のプロパティとして持つか迷う
				parameter.Filter = filter.ToODataFilterString(homeType: homeType);
				Debug.WriteLine("filter: " + parameter.Filter);
			}

			return await _azureSearchClient.IndexClient.Documents.SearchAsync<SearchItem>(searchText, parameter);
		}

		/// <summary>
		/// ここから離れるとき
		/// </summary>
		/// <param name="parameters"></param>
		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		/// <summary>
		/// こっちに来たとき
		/// - 検索キーワードでの検索
		/// - フィルター適用後の検索
		/// </summary>
		/// <param name="parameters"></param>
		public void OnNavigatedTo(NavigationParameters parameters)
		{
			bool wantToSearch = false;

			//search text
			string searchText = parameters.GetValueOrDefault<string>(SearchPageViewModel.SearchTextParameterKey);
			if (!string.IsNullOrEmpty(searchText))
			{
				wantToSearch = true;
				this.SearchText = searchText;
			}

			//search filter
			SearchFilter filter = parameters.GetValueOrDefault<SearchFilter>(SearchPageViewModel.SearchFilterParameterKey);
			if (filter != null)
			{
				wantToSearch = true;
				this._filter = filter;
			}

			if (wantToSearch)
			{
				//検索（このとき、画面のタイプの更新は行う）
				this.Search(this.SearchText, this._filter, updateHomeTypeFacetStatus: true).FireAndForget();
			}
		}
	}
}
