﻿using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Pages
{
	public partial class BordersPage
	{
		public BordersPage()
		{
			InitializeComponent();

			UpdateBackground();
			UpdateBorder();
			UpdateCornerRadius();
		}

		void OnBackgroundChanged(object sender, TextChangedEventArgs e)
		{
			UpdateBackground();
		}

		void OnBorderChanged(object sender, TextChangedEventArgs e)
		{
			UpdateBorder();
		}

		void OnBorderWidthChanged(object sender, ValueChangedEventArgs e)
		{
			UpdateBorder();
		}

		void OnCornerRadiusChanged(object sender, ValueChangedEventArgs e)
		{
			UpdateCornerRadius();
		}

		void UpdateBackground()
		{
			var startColor = GetColorFromString(BackgroundStartColor.Text);
			var endColor = GetColorFromString(BackgroundEndColor.Text);

			BackgroundStartColor.BackgroundColor = startColor;
			BackgroundEndColor.BackgroundColor = endColor;

			BorderView.Background = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0),
				GradientStops = new GradientStopCollection
				{
					new Microsoft.Maui.Controls.GradientStop { Color = startColor, Offset = 0.0f },
					new Microsoft.Maui.Controls.GradientStop { Color = endColor, Offset = 0.9f }
				}
			};
		}

		void UpdateBorder()
		{
			var startColor = GetColorFromString(BorderStartColor.Text);
			var endColor = GetColorFromString(BorderEndColor.Text);

			BorderStartColor.BackgroundColor = startColor;
			BorderEndColor.BackgroundColor = endColor;

			BorderView.BorderBrush = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 0),
				GradientStops = new GradientStopCollection
				{
					new Microsoft.Maui.Controls.GradientStop { Color = startColor, Offset = 0.0f },
					new Microsoft.Maui.Controls.GradientStop { Color = endColor, Offset = 0.9f }
				}
			};

			BorderView.BorderWidth = BorderWidthSlider.Value;
		}

		void UpdateCornerRadius()
		{
			BorderView.CornerRadius = new CornerRadius(TopLeftCornerSlider.Value, TopRightCornerSlider.Value,
				BottomLeftCornerSlider.Value, BottomRightCornerSlider.Value);
		}

		Color GetColorFromString(string value)
		{
			if (string.IsNullOrEmpty(value))
				return Colors.Transparent;

			try
			{
				return Color.FromArgb(value);
			}
			catch (Exception)
			{
				return Colors.Transparent;
			}
		}
	}
}