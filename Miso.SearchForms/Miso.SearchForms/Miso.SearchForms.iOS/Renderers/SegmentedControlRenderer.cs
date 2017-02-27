using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

/*
	 * LICENSE:
	 * https://github.com/chrispellett/Xamarin-Forms-SegmentedControl/blob/master/LICENSE.txt
	 */

[assembly: ExportRenderer(typeof(Miso.SearchForms.Controls.SegmentedControl), typeof(Miso.SearchForms.iOS.Renderers.SegmentedControlRenderer))]
namespace Miso.SearchForms.iOS.Renderers
{
	public class SegmentedControlRenderer : ViewRenderer<Controls.SegmentedControl, UIKit.UISegmentedControl>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Controls.SegmentedControl> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				var segmentedControl = new UIKit.UISegmentedControl();

				for (var i = 0; i < e.NewElement.Children.Count; i++)
				{
					segmentedControl.InsertSegment(e.NewElement.Children[i].Text, i, false);
				}

				segmentedControl.ValueChanged += ValueChanged;

				SetNativeControl(segmentedControl);
			}
		}

		private void ValueChanged(object sender, EventArgs eventArgs)
		{
			var segmentedControl = (UISegmentedControl)sender;
			int selectedIndex = Convert.ToInt32(segmentedControl.SelectedSegment); //nintはlongに変換すべきだが、SegmentedControlの選択肢に限ってはありえないのでintへ
			Element?.SelectedChange(newSelectedIndex: selectedIndex);
		}
		
		protected override void Dispose(bool disposing)
		{
			if (Control != null)
				Control.ValueChanged -= ValueChanged;

			base.Dispose(disposing);
		}
	}
}
