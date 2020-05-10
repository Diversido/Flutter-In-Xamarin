# Overview

This is an example of embedding Flutter into Xamarin

The reasons are not so important :)

# Embed Flutter

The first major step is prepare flutter project to be embedded

Mostly follow the flutter documentation on embedding flutter app into the native project:  
https://flutter.dev/docs/development/add-to-app/ios/project-setup

### 1. Create Flutter module

```
flutter create --template module flutter_module
```

### 2. Export flutter framework

```
flutter build ios-framework
flutter build aar
```



# Create Xamarin iOS bindings

Mostly follow Xamarin steps for importing native library (framework)

### 1. Create project

https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-c-libraries

Create Xamarin iOS Binding project

### 2. Add BOTH frameworks as native libraries

Check folder `flutter_module/build/ios/framework/Debug`:

- `App.framework`
- `Flutter.framework`

### 3. Bind basic Flutter methods

So far we just need a few method from the Flutter engine:

- `FlutterAppDelegate`
- `FlutterEngine`
- `FlutterViewController`

Add methods one-by-one as they needed rather than exporting all method (using `sharpie` tool)

Note: there are no methods to bind from `App.framework`, it just contains the `.dart` files

Note: it is important to add both frameworks to the single binding project. Create 2 Xamarin bindings projects for every framework does not work for me.



# Use in Xamarin iOS project

```csharp
	public class AppDelegate : FlutterAppDelegate
	{
		public static FlutterEngine FlutterEngine { get; private set; }

		[Export ("application:didFinishLaunchingWithOptions:")]
		public bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			FlutterEngine = new FlutterEngine ("My Flutter Engine");
			FlutterEngine.Run ();

			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method
			return true;
		}
	}
```

```csharp
// somewhere in the Xamarin app, when want to show a Flutter controlelr
	void ButtonClicked ()
	{
		var vc = new FlutterViewController (AppDelegate.FlutterEngine, null, null);
		PresentViewController (vc, true, null);
	}
```



# Create Xamarin Android bindings

### 1. Create project

Create Xamarin Android Binding project

### 2. Add Flutter AAR

Check folder `flutter_module/build/host/outputs/repo/com/example/flutter_module/`:

- `flutter_release/flutter_release-1.0.aar`

### 3. Copy Flutter Dependencies

Ensure `flutter build aar` has been invoked, as this command will cache the dependencies in the local gradle cache folder.

1. Go to `~/.gradle/caches/modules-2/files-2.1/io.flutter/`
2. Copy jar-files from the next folders to the `Xamarin.Bindings.Android/Jars`:
-  `arm64_v8a_release`
-  `armeabi_v7a_release`
-  `x86_64_release`
-  `flutter_embedding_release`

The files are:
- `arm64_v8a_release-1.0.0-540786dd51f112885a89792d678296b95e6622e5.jar`
-  `armeabi_v7a_release-1.0.0-540786dd51f112885a89792d678296b95e6622e5.jar`
- `x86_64_release-1.0.0-540786dd51f112885a89792d678296b95e6622e5.jar`
- `flutter_embedding_release-1.0.0-540786dd51f112885a89792d678296b95e6622e5.jar`

NOTE: this kind of build will work only on `x86_64` emulators and crash on `x86` emulators.  
To support `x86` emulators you need to reference one more library with `x86_release` flutter support.

### 4. Adjust library classes visibility

So far we just need a few adjustments to `Metadata.xml`:

```xml
    <attr path="/api/package[@name='io.flutter.embedding.android']/class[@name='FlutterActivityAndFragmentDelegate']" name="visibility">public</attr>
    <remove-node path="/api/package[@name='io.flutter.embedding.engine.dart']/interface[@name='DartExecutor.IsolateServiceIdListener']"/>
```


# Use in Xamarin Android project

Ensure to migrate the project to `AndroidX`. There was a few issues with `Android.Support` so I've migrated to `AndroidX`.

```csharp
	public class MainActivity : AppCompatActivity
	{
        static FlutterEngine flutterEngine;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// ...

			// Create Flutter Engine
            if (flutterEngine == null)
            {
				flutterEngine = new FlutterEngine(this);

				flutterEngine.DartExecutor.ExecuteDartEntrypoint(
                    DartEntrypoint.CreateDefault()
                );

				FlutterEngineCache.Instance.Put("my_engine", flutterEngine);
			}
		}
	}
```

```csharp
// somewhere in the Xamarin app, when want to show a Flutter controlelr
	void ButtonClicked ()
	{
		StartActivity (FlutterActivity.WithCachedEngine ("my_engine").Build (this));
	}
```


# Known issues

Note: could not make **hot reload** to work
