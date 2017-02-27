using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Miso.SearchForms.Controls;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Miso.SearchForms.Controls.SegmentedControl), typeof(Miso.SearchForms.Droid.Renderers.SegmentedControlRenderer))]
namespace Miso.SearchForms.Droid.Renderers
{
	public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, RadioGroup>
	{
		private RadioGroup NativeControl { get { return this.Control; } }

		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
		{
			System.Diagnostics.Debug.WriteLine("OnElementChanged e.NewElement" + (e.NewElement));
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.TextChanged -= this.TextChanged;
			}

			if (this.Control == null)
			{
				var layoutInflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

				e.NewElement.TextChanged += this.TextChanged;

				var g = new RadioGroup(Context);
				g.Orientation = Orientation.Horizontal;
				g.CheckedChange += this.CheckedChange;

				for (var i = 0; i < e.NewElement.Children.Count; i++)
				{
					SegmentedControlOption o = e.NewElement.Children[i];
					var v = (SegmentedControlButton)layoutInflater.Inflate(Resource.Layout.SegmentedControl, null);
					v.Text = o.Text;
					if (i == 0)
						v.SetBackgroundResource(Resource.Drawable.segmented_control_first_background);
					else if (i == e.NewElement.Children.Count - 1)
						v.SetBackgroundResource(Resource.Drawable.segmented_control_last_background);
					g.AddView(v);
				}

				SetNativeControl(g);
			}
		}

		/// <summary>
		/// ネイティブのラジオボタンのチェックが変更されたとき、Formsへ送信する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs args)
		{
			var radioGroup = (RadioGroup)sender;
			var checkedRadioButtonId = radioGroup.CheckedRadioButtonId;
			var checkedRadioButton = radioGroup.FindViewById(checkedRadioButtonId);
			int selectedIndex = radioGroup.IndexOfChild(checkedRadioButton);

			System.Diagnostics.Debug.WriteLine("radioId:" + selectedIndex);

			var selectedButton = (RadioButton)radioGroup.GetChildAt(selectedIndex);
			var selectedText = (String)selectedButton.Text;
			//Forms側へ送信
			Element?.SelectedChange(selectedIndex);
		}

		/// <summary>
		/// テキストが変更されたとき、ネイティブの値を変更する
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextChanged(object sender, Miso.SearchForms.Controls.SegmentedControlEvents.TextChangedEventArgs e)
		{
			var element = sender as SegmentedControl;
			var textChangedChild = (RadioButton)NativeControl.GetChildAt(index: e.Index);
			textChangedChild.Text = e.NewText;
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged " + e.PropertyName);
			base.OnElementPropertyChanged(sender, e);
		}

		protected override void Dispose(bool disposing)
		{
			if (Control != null)
			{
				Control.CheckedChange -= CheckedChange;
			}
			if (Element != null)
			{
				Element.TextChanged -= TextChanged;
			}

			base.Dispose(disposing);
		}
	}
}