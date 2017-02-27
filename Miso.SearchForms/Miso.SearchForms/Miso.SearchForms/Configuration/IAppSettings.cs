using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Configuration
{
	public interface IAppSettings
	{
		string AzureSearchServiceName { get; }
		string AzureSearchAPIKey { get; }
		string AzureSearchIndexName { get; }
	}
}
