﻿using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	public static class TextViewExtensions
	{
		public static void UpdateText(this TextView textView, Label label)
		{
			switch (label.TextType)
			{
				case TextType.Text:
					if (label.FormattedText != null)
						textView.TextFormatted = label.FormattedText.ToAttributed(label.ToFont(), label.TextColor, textView);
					else
						textView.Text = TextTransformUtilites.GetTransformedText(label.Text, label.TextTransform);
					break;
				case TextType.Html:
					textView.UpdateTextHtml(label);
					break;
			}
		}
	}
}
