using Miso.SearchForms.Common;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Miso.SearchForms.Tests.Common
{
	public class NavigationParametersExtensionsTest
	{
		[Fact]
		public void GetOrDefaultValueExpectException()
		{
			var p = new NavigationParameters();
			string key = "mykey";
			p.Add(key, new Person());

			Assert.Throws<InvalidCastException>(() => NavigationParametersExtensions.GetValueOrDefault<Book>(p, key));
		}

		private class Person
		{

		}
		private class Book
		{

		}
	}
}
