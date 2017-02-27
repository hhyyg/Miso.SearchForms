using Microsoft.Practices.Unity.Utility;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miso.SearchForms.Common
{
	public static class NavigationParametersExtensions
	{
		public static TValue GetValueOrDefault<TValue>(this NavigationParameters navigationParameters, string key)
		{
			if (navigationParameters == null)
				throw new ArgumentNullException(nameof(navigationParameters));
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			
			object value;
			return navigationParameters.TryGetValue(key, out value) ? (TValue)value : default(TValue);
		}
	}
}
