using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Miso.SearchForms.Common;
using Miso.SearchForms.Configuration;
using Miso.SearchForms.Models;
using Miso.SearchForms.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Miso.SearchForms.Common.TaskApplicationExtensions;

namespace Miso.SearchForms.ViewModels
{
	/// <summary>
	/// キーワードからサジェストを表示する画面
	/// </summary>
	public class SuggestionsPageViewModel : BindableBase, INavigationAware
	{
		public SuggestionsPageViewModel(
			IPageDialogService pageDialogService,
			INavigationService navigationService,
			IAzureSearchClient azureSearchClient)
		{
			this._pageDialogService = pageDialogService;
			this._navigationService = navigationService;
			this._azureSearchClient = azureSearchClient;
			this.ItemList = new ObservableCollection<SearchItem>();
			this.SearchCommand = new DelegateCommand(() => this.NavigateToSearchPage(this.SearchText).FireAndForget());
			this.OnListViewItemSelectedCommand = new DelegateCommand<SearchItem>((item) => this.OnListViewItemSelected(item).FireAndForget());
			this.OnSearchBarTextChangedCommand = new DelegateCommand<TextChangedEventArgs>((args) => this.OnSearchBarTextChanged(args).FireAndForget());
		}

		private IPageDialogService _pageDialogService { get; }
		private INavigationService _navigationService { get; }
		private IAzureSearchClient _azureSearchClient { get; }

		/// <summary>
		/// 検索実行
		/// </summary>
		public DelegateCommand SearchCommand { get; }

		/// <summary>
		/// サジェスト結果 選択
		/// </summary>
		public DelegateCommand<SearchItem> OnListViewItemSelectedCommand { get; }
		
		/// <summary>
		/// サジェスト結果
		/// </summary>
		public ObservableCollection<SearchItem> ItemList { get; private set; }

		/// <summary>
		/// キーワード変更
		/// </summary>
		public DelegateCommand<TextChangedEventArgs> OnSearchBarTextChangedCommand { get; }

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

		private bool CanSearchCommand
		{
			get {  return !string.IsNullOrEmpty(this.SearchText); }
		}

		/// <summary>
		/// キーワード変更時
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private Task OnSearchBarTextChanged(TextChangedEventArgs args)
		{
			return Task.Run(() =>
			{
				//キーワードが入力されたらサジェストを表示する
				bool isFook = (args != null && args.NewTextValue.Length >= 3);
				if (isFook && !this.IsBusy)
				{
					Suggest(args.NewTextValue).FireAndForget();
				}
			});
		}
		
		/// <summary>
		/// using で IsBusy の切り替えができるようにするためのメソッド
		/// </summary>
		IDisposable BeginBusy()
		{
			this.IsBusy = true;
			return Disposable.Create(() => this.IsBusy = false);
		}

		/// <summary>
		/// サジェスト結果 選択時
		/// </summary>
		/// <param name="searchItem"></param>
		/// <returns></returns>
		private async Task OnListViewItemSelected(SearchItem searchItem)
		{
			//結果を選択したら、次の画面にてそのキーワードで検索する
			if (searchItem != null)
			{
				await NavigateToSearchPage(searchText: searchItem.City);
			}
		}

		/// <summary>
		/// 検索ページに行き、キーワードで検索する
		/// </summary>
		/// <returns></returns>
		private async Task NavigateToSearchPage(string searchText)
		{
			if (this.CanSearchCommand == false)
			{
				await this._pageDialogService.DisplayAlertAsync(
					title: "please, enter search term.",
					message: "ex) Seattle, Issaquah, Burien",
					cancelButton: "Close");
				return;
			}

			var parameter = new NavigationParameters();
			parameter.Add(SearchPageViewModel.SearchTextParameterKey, searchText); 

			await this._navigationService.NavigateAsync(nameof(SearchPage), parameter);
		}

		private async Task<DocumentSuggestResult<SearchItem>> SuggetAsync(string text)
		{
			var parameters = new SuggestParameters()
			{
				UseFuzzyMatching = false,
				//HighlightPreTag = "[",
				//HighlightPostTag = "]",
				MinimumCoverage = 100,
				Top = 10,
				SearchFields = new string[] { "city" }
			};
			return await _azureSearchClient.IndexClient.Documents.SuggestAsync<SearchItem>(text, "sg", parameters);
		}
		
		/// <summary>
		/// サジェストを検索する
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private async Task Suggest(string text)
		{
			this.ItemList.Clear();
			List<SearchItem> suggestResults;

			using (BeginBusy())
			{
				DocumentSuggestResult<SearchItem> suggestionResults = await SuggetAsync(text);
				suggestResults = suggestionResults.Results
					.Select(x => x.Document.City)
					.Distinct()
					.Select(x => new SearchItem() { City = x })
					.ToList();
			}

			foreach(var item in suggestResults)
			{
				this.ItemList.Add(item);
			}
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
		}
	}
}
