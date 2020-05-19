using Foundation;
using System;
using System.Diagnostics;
using UIKit;
using XamWithFlut.Binding.Flutter;

namespace XamWithFlut.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var button = new UIButton (UIButtonType.System);
			button.TranslatesAutoresizingMaskIntoConstraints = false;
			button.SetTitle ("Open Flutter View", UIControlState.Normal);
			button.TouchUpInside += delegate { ButtonClicked (); };

			View.AddSubview (button);

			button.WidthAnchor.ConstraintEqualTo (180).Active = true;
			button.HeightAnchor.ConstraintEqualTo (44).Active = true;
			View.CenterXAnchor.ConstraintEqualTo (button.CenterXAnchor).Active = true;
			View.CenterYAnchor.ConstraintEqualTo (button.CenterYAnchor).Active = true;
		}

		void ButtonClicked ()
		{
			var vc = new FlutterViewController (AppDelegate.FlutterEngine, null, null);
			PresentViewController (vc, true, null);

			var channel = FlutterMethodChannel.MethodChannelWithName("diversido.io/main", vc.BinaryMessenger);

			channel.InvokeMethod("reset", NSNumber.FromNInt(11));

			channel.SetMethodCallHandler((FlutterMethodCall call, FlutterResult result) =>
			{
				if (call.Method == "increment")
				{
					var counter = (int)(NSNumber)call.Arguments;

					try
					{
						result((NSNumber)(counter + 1));
					}
					catch (Exception e)
					{
						Debug.WriteLine(e);
					}
				}
			});

		}
	}
}