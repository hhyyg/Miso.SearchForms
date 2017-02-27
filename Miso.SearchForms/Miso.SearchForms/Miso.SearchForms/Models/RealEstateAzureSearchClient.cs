using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.Search;
using Miso.SearchForms.Configuration;

namespace Miso.SearchForms.Models
{
	/// <summary>
	/// Azure Search Client for App
	/// </summary>
	public class RealEstateAzureSearchClient : IAzureSearchClient
	{
		public RealEstateAzureSearchClient(Configuration.IAppSettings appSettings)
		{
			_indexClient = new SearchIndexClient(
				searchServiceName: appSettings.AzureSearchServiceName,
				indexName: appSettings.AzureSearchIndexName,
				credentials: new SearchCredentials(apiKey: appSettings.AzureSearchAPIKey));
		}
		
		private readonly SearchIndexClient _indexClient = null;

		public SearchIndexClient IndexClient { get { return _indexClient; } }
	}
}
