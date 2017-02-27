using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Miso.SearchForms.Controls
{
	public class SegmentedControlOption : View
	{
		public int Index { get; set; }

		public static readonly BindableProperty TextProperty = BindableProperty.Create(
			nameof(Text),
			typeof(string),
			typeof(SegmentedControlOption),
			propertyChanged: OnTextChanged);

		/// <summary>
		/// Textの値が変更されたとき
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
		{
			Debug.WriteLine($"old:{oldValue}, new: {newValue}");
			//ここで、親SegmentedControlに通知し、ネイティブに通知し、画面表示のテキストを変更する

			var option = (SegmentedControlOption)bindable;
			option.TextChanged?.Invoke(
				option,
				new SegmentedControlEvents.TextChangedEventArgs()
				{
					OldText = oldValue as string,
					NewText = newValue as string,
					Index = option.Index
				});
		}

		/// <summary>
		/// モデル側のTextの値が変更されたときを親に購読してもらうためのイベント
		/// </summary>
		public event EventHandler<SegmentedControlEvents.TextChangedEventArgs> TextChanged;

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}
