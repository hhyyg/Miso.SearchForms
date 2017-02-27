using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

/*
	 * LICENSE:
	 * https://github.com/chrispellett/Xamarin-Forms-SegmentedControl/blob/master/LICENSE.txt
	 */

namespace Miso.SearchForms.Droid
{
	[Activity(Label = "Miso.SearchForms", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			PCLAppConfig.ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);

			LoadApplication(new App());
		}
	}
}

