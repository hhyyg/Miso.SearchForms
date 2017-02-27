using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Models.SearchFacets
{
	public struct ItemCountPair
	{
		public ItemCountPair(string name, int count)
		{
			Name = name;
			Count = count;
		}

		public string Name;
		public int Count;

		public override string ToString()
		{
			return (Count <= 0) ? Name : $"{Name}({Count})";
		}
	}
}
