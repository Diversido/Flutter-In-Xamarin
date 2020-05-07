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
```

# Create Xamarin bindings

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

# Known issues

Note: could not make **hot reload** to work
