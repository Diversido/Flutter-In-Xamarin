using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using IO.Flutter.Embedding.Android;
using IO.Flutter.Embedding.Engine;
using static IO.Flutter.Embedding.Engine.Dart.DartExecutor;

namespace XamWithFlut.Droid
{
	[Activity (Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
        static FlutterEngine flutterEngine;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Xamarin.Essentials.Platform.Init (this, savedInstanceState);
			SetContentView (Resource.Layout.activity_main);

			AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById< AndroidX.AppCompat.Widget.Toolbar> (Resource.Id.toolbar);
			SetSupportActionBar (toolbar);

			FloatingActionButton fab = FindViewById<FloatingActionButton> (Resource.Id.fab);
			fab.Click += FabOnClick;
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu_main, menu);
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			int id = item.ItemId;
			if (id == Resource.Id.action_settings)
			{
				return true;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void FabOnClick (object sender, EventArgs eventArgs)
		{
			//View view = (View)sender;
			//Snackbar.Make (view, "Replace with your own action", Snackbar.LengthLong)
			//	.SetAction ("Action", (Android.Views.View.IOnClickListener)null).Show ();

            if (flutterEngine == null)
            {
				flutterEngine = new FlutterEngine(this);

				flutterEngine.DartExecutor.ExecuteDartEntrypoint(
                    DartEntrypoint.CreateDefault()
                );

				FlutterEngineCache.Instance.Put("my_engine", flutterEngine);
			}

			StartActivity (FlutterActivity.WithCachedEngine ("my_engine").Build (this));
		}

		public override void OnRequestPermissionsResult (int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult (requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}
	}
}

