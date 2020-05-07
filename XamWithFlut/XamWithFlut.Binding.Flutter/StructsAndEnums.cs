using System;
using ObjCRuntime;

namespace XamWithFlut.Binding.Flutter
{
	[Native]
	public enum FlutterStandardDataType : ulong
	{
		UInt8,
		Int32,
		Int64,
		Float64
	}

	public enum FlutterPlatformViewGestureRecognizersBlockingPolicy : uint
	{
		Eager,
		WaitUntilTouchesEnded
	}
}
