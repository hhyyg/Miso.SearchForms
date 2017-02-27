using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms.Converters
{
	//https://github.com/PrismLibrary/Prism/blob/master/docs/Xamarin-Forms/6-EventToCommandBehavior.md

	public class ItemTappedEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var itemTappedEventArgs = value as ItemTappedEventArgs;
			if (itemTappedEventArgs == null)
			{
				throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
			}
			return itemTappedEventArgs.Item;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
