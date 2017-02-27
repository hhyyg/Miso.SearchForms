using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Miso.SearchForms.Models
{
	public interface IAzureSearchClient
	{
		SearchIndexClient IndexClient { get; }
	}
}
