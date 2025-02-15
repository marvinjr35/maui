#nullable enable
using System;
using Windows.Graphics.Display;
using Windows.Graphics.Display.Core;
using Windows.System.Display;

namespace Microsoft.Maui.Essentials
{
	partial class DeviceDisplayImplementation : IDeviceDisplay
	{
		readonly object locker = new object();
		DisplayRequest? displayRequest;

		public event EventHandler<DisplayInfoChangedEventArgs>? MainDisplayInfoChanged;

		public bool KeepScreenOn
		{
			get
			{
				lock (locker)
				{
					return displayRequest != null;
				}
			}

			set
			{
				lock (locker)
				{
					if (value)
					{
						if (displayRequest == null)
						{
							displayRequest = new DisplayRequest();
							displayRequest.RequestActive();
						}
					}
					else
					{
						if (displayRequest != null)
						{
							displayRequest.RequestRelease();
							displayRequest = null;
						}
					}
				}
			}
		}

		public DisplayInfo GetMainDisplayInfo() =>
			GetMainDisplayInfo(null);

		DisplayInfo GetMainDisplayInfo(DisplayInformation? di = null)
		{
			di ??= DisplayInformation.GetForCurrentView();

			var rotation = CalculateRotation(di);
			var perpendicular =
				rotation == DisplayRotation.Rotation90 ||
				rotation == DisplayRotation.Rotation270;

			var w = di.ScreenWidthInRawPixels;
			var h = di.ScreenHeightInRawPixels;

			var hdi = HdmiDisplayInformation.GetForCurrentView();
			var hdm = hdi?.GetCurrentDisplayMode();

			return new DisplayInfo(
				width: perpendicular ? h : w,
				height: perpendicular ? w : h,
				density: di.LogicalDpi / 96.0,
				orientation: CalculateOrientation(di),
				rotation: rotation,
				rate: (float)(hdm?.RefreshRate ?? 0));
		}

		public void StartScreenMetricsListeners()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				var di = DisplayInformation.GetForCurrentView();

				di.DpiChanged += OnDisplayInformationChanged;
				di.OrientationChanged += OnDisplayInformationChanged;
			});
		}

		public void StopScreenMetricsListeners()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				var di = DisplayInformation.GetForCurrentView();

				di.DpiChanged -= OnDisplayInformationChanged;
				di.OrientationChanged -= OnDisplayInformationChanged;
			});
		}

		void OnDisplayInformationChanged(DisplayInformation di, object args)
		{
			var metrics = GetMainDisplayInfo(di);
			MainDisplayInfoChanged?.Invoke(this, new DisplayInfoChangedEventArgs(metrics));
		}

		DisplayOrientation CalculateOrientation(DisplayInformation di)
		{
			switch (di.CurrentOrientation)
			{
				case DisplayOrientations.Landscape:
				case DisplayOrientations.LandscapeFlipped:
					return DisplayOrientation.Landscape;
				case DisplayOrientations.Portrait:
				case DisplayOrientations.PortraitFlipped:
					return DisplayOrientation.Portrait;
			}

			return DisplayOrientation.Unknown;
		}

		static DisplayRotation CalculateRotation(DisplayInformation di)
		{
			var native = di.NativeOrientation;
			var current = di.CurrentOrientation;

			if (native == DisplayOrientations.Portrait)
			{
				switch (current)
				{
					case DisplayOrientations.Landscape:
						return DisplayRotation.Rotation90;
					case DisplayOrientations.Portrait:
						return DisplayRotation.Rotation0;
					case DisplayOrientations.LandscapeFlipped:
						return DisplayRotation.Rotation270;
					case DisplayOrientations.PortraitFlipped:
						return DisplayRotation.Rotation180;
				}
			}
			else if (native == DisplayOrientations.Landscape)
			{
				switch (current)
				{
					case DisplayOrientations.Landscape:
						return DisplayRotation.Rotation0;
					case DisplayOrientations.Portrait:
						return DisplayRotation.Rotation270;
					case DisplayOrientations.LandscapeFlipped:
						return DisplayRotation.Rotation180;
					case DisplayOrientations.PortraitFlipped:
						return DisplayRotation.Rotation90;
				}
			}

			return DisplayRotation.Unknown;
		}
	}
}
