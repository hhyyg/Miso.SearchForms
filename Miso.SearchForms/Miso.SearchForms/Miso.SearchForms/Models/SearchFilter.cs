using Miso.SearchForms.Models.SearchFacets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Models
{
	public class SearchFilter
	{
		public static SearchFilter Default = CreateDefaultSearchFilter();

		public int BedsMin { get; set; }
		public int BedsMax { get; set; }
		public bool IncludeStatusSale { get; set; }
		public bool IncludeStatusPending { get; set; }
		public bool IncludeStatusSold { get; set; }

		private const string beds = "beds";
		private const string status = "status";
		private const string price = "price";
		private const string type = "type";

		/// <summary>
		/// create filter OData syntax
		/// https://docs.microsoft.com/en-us/rest/api/searchservice/Search-Documents
		/// </summary>
		/// <param name="homeType">typeの条件</param>
		/// <returns></returns>
		public string ToODataFilterString(HomeType homeType = HomeType.All)
		{
			var builder = new StringBuilder();

			builder.Append($"{beds} ge {BedsMin} and {beds} le {BedsMax}");

			if (homeType != HomeType.All)
			{
				builder.Append($" and {type} eq '{Enum.GetName(typeof(HomeType), homeType)}'");
			}
			
			if ((IncludeStatusSale && IncludeStatusSale && IncludeStatusSold) == false)
			{
				builder.Append($" and ({CreateStatusFilterString()}) ");
			}
			//else: all status
			
			return builder.ToString();
		}

		private string CreateStatusFilterString()
		{
			if (!IncludeStatusSale && !IncludeStatusPending && !IncludeStatusSold)
				return $"{status} eq 'noresults'";

			var itemFilter = new List<string>();

			if (IncludeStatusPending)
				itemFilter.Add($"{status} eq 'pending'");
			if (IncludeStatusSale)
				itemFilter.Add($"{status} eq 'active'");
			if (IncludeStatusSold)
				itemFilter.Add($"{status} eq 'sold'");

			return string.Join(" or ", itemFilter);
		}

		private static SearchFilter CreateDefaultSearchFilter()
		{
			return new SearchFilter()
			{
				BedsMin = 0,
				BedsMax = 6,
				IncludeStatusPending = true,
				IncludeStatusSale = true,
				IncludeStatusSold = true,
			};
		}
	}
}
