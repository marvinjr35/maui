﻿using System;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui
{
	public interface IFontManager
	{
		UIFont DefaultFont { get; }

		UIFont GetFont(Font font, double defaultFontSize = 0);
	}
}