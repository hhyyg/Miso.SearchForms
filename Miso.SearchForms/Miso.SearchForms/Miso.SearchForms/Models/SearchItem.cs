using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Models
{
	/// <summary>
	/// 検索結果のアイテム
	/// </summary>
	public class SearchItem
	{
		public int SearchScore { get; set; }
		public string ListingId { get; set; }
		public int Beds { get; set; }
		public int Baths { get; set; }
		public string Description { get; set; }
		public string Description_de { get; set; }
		public string Description_fr { get; set; }
		public string Description_it { get; set; }
		public string Description_es { get; set; }
		public string Description_pl { get; set; }
		public string Description_nl { get; set; }
		public int Sqft { get; set; }
		public int DaysOnMarket { get; set; }
		public string Status { get; set; }
		public string Source { get; set; }
		public string Number { get; set; }
		public string Street { get; set; }
		public string Unit { get; set; }
		public string Type { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string CountryCode { get; set; }
		public string PostCode { get; set; }
		public Location Location { get; set; }
		public int Price { get; set; }
		public string Thumbnail { get; set; }
		public string[] Tags { get; set; }
	}
	
	public class Location
	{
		public string Type { get; set; }
		public float[] Coordinates { get; set; }
		public Crs Crs { get; set; }
	}

	public class Crs
	{
		public string Type { get; set; }
		public Properties Properties { get; set; }
	}

	public class Properties
	{
		public string Name { get; set; }
	}

}
