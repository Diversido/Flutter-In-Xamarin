#!/bin/bash

IOS_SDK=iphoneos13.4

# build flutter module
#flutter build ios-framework


# Sharpify
sharpie bind \
	-sdk $IOS_SDK \
	-output XamWithFlut/XamWithFlut.Binding.Flutter \
	-namespace XamWithFlut.Binding.Flutter \
	-framework flutter_module/build/ios/framework/Debug/Flutter.framework
#sharpie bind -sdk $IOS_SDK -namespace Xamarin.Bindings.Flutter.App -scope flutter_module/build/ios/framework/Release/App.framework/Headers

