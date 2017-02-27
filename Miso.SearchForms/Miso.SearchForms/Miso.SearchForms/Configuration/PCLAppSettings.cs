using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Configuration
{
	public class PCLAppSettings : IAppSettings
	{
		public string AzureSearchAPIKey
		{
			get
			{
				return PCLAppConfig.ConfigurationManager.AppSettings["Miso.SearchForms.AzureSearch.APIKey"];
			}
		}

		public string AzureSearchIndexName
		{
			get
			{
				return PCLAppConfig.ConfigurationManager.AppSettings["Miso.SearchForms.AzureSearch.IndexName"];
			}
		}

		public string AzureSearchServiceName
		{
			get
			{
				return PCLAppConfig.ConfigurationManager.AppSettings["Miso.SearchForms.AzureSearch.ServiceName"];
			}
		}
	}
}
