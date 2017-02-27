using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms.Converters
{
	/// <summary>
	/// TextChangedEventArgs をそのまま渡すコンバーター
	/// </summary>
	public class TextChangedEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var args = value as TextChangedEventArgs;
			if (args == null)
			{
				throw new ArgumentException($"Expected value to be of type {nameof(TextChangedEventArgs)}", nameof(value));
			}
			return args;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
