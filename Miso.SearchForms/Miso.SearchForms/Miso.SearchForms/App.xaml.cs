using Microsoft.Practices.Unity;
using Miso.SearchForms.Configuration;
using Miso.SearchForms.Models;
using Miso.SearchForms.Views;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms
{
	public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }
		protected override void OnInitialized()
		{
			InitializeComponent();
			NavigationService.NavigateAsync("HomePage");
		}

		protected override void RegisterTypes()
		{
			Container.RegisterType(typeof(IAppSettings), typeof(PCLAppSettings), null, new ContainerControlledLifetimeManager());
			Container.RegisterType(typeof(IAzureSearchClient), typeof(RealEstateAzureSearchClient), null, new ContainerControlledLifetimeManager());

			Container.RegisterTypeForNavigation<HomePage>();
			Container.RegisterTypeForNavigation<SearchPage>();
			Container.RegisterTypeForNavigation<SuggestionsPage>();
			Container.RegisterTypeForNavigation<SearchDetailsPage>();
			Container.RegisterTypeForNavigation<FilterPage>();
			Container.RegisterTypeForNavigation<NavigationPage>();
		}
	}
}
