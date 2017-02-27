using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Miso.SearchForms.Controls
{
	/*
	 * LICENSE:
	 * https://github.com/chrispellett/Xamarin-Forms-SegmentedControl/blob/master/LICENSE.txt
	 */

	public class SegmentedControl : View, IViewContainer<SegmentedControlOption>
	{
		public SegmentedControl()
		{
			var child = new ObservableCollection<SegmentedControlOption>();
			child.CollectionChanged += (sender, e) => CollectionChanged(this, e);
			this.Children = child;
		}

		public IList<SegmentedControlOption> Children { get; set; }

		public static readonly BindableProperty SelectionChangedProperty =
			BindableProperty.Create(
				nameof(Command),
				typeof(ICommand),
				typeof(SegmentedControl));

		/// <summary>
		/// 選択されたときに起動するコマンド
		/// </summary>
		public ICommand SelectionChangedCommand
		{
			get { return (ICommand)GetValue(SelectionChangedProperty); }
			set { SetValue(SelectionChangedProperty, value); }
		}

		/// <summary>
		/// 子要素のCollectionChanged
		/// </summary>
		/// <param name="control"></param>
		/// <param name="e"></param>
		static void CollectionChanged(SegmentedControl control, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					AddChild(control, e);
					break;
				case NotifyCollectionChangedAction.Remove:
					throw new NotImplementedException();
				default: //Move, Replace, Reset
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// 子要素が追加されたとき
		/// </summary>
		/// <param name="control"></param>
		/// <param name="e"></param>
		static void AddChild(SegmentedControl control, NotifyCollectionChangedEventArgs e)
		{
			int startIndex = control.Children.Count - 1;
			foreach (var newItem in e.NewItems)
			{
				var child = (SegmentedControlOption)newItem;
				child.Index = startIndex++;//TextChangedのときにIndex取得するため、Childに設定する
				child.TextChanged += (o, args) => ChildTextChanged(o, args);//ネイティブに通知するため、子のTextChangedイベントを購読する
				child.Parent = control;//親を参照できるように設定
			}
		}

		/// <summary>
		/// 子要素のBindingContextを設定する
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			
			foreach (SegmentedControlOption child in Children)
			{
				SetInheritedBindingContext(child, BindingContext);
			}
		}

		/// <summary>
		/// 子のTextChangedを、ネイティブコントロールに通知する
		/// </summary>
		static void ChildTextChanged(object sender, SegmentedControlEvents.TextChangedEventArgs e)
		{
			var control = ((SegmentedControlOption)sender).Parent as SegmentedControl;
			control.TextChanged?.Invoke(control, e); //親をsenderとするのが一般的？
		}

		/// <summary>
		/// ネイティブに購読してもらうための、子のTextが変更されたことを送信するイベント
		/// </summary>
		internal event EventHandler<SegmentedControlEvents.TextChangedEventArgs> TextChanged;
		
		/// <summary>
		/// ネイティブ側で選択が変更されたとき
		/// </summary>
		/// <param name="newSelectedIndex"></param>
		internal void SelectedChange(int newSelectedIndex)
		{
			if (SelectionChangedProperty != null)
				SelectionChangedCommand.Execute(parameter: (object)newSelectedIndex);
		}
	}
}