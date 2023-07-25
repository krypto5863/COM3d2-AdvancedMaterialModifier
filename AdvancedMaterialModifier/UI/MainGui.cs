using System;
using UnityEngine;

namespace AdvancedMaterialModifier.UI
{
	public static class MainGui
	{
		public const int WindowId = 432543253;

		internal static Rect WindowRect = new Rect(Screen.width / 3f, Screen.height / 4f, Screen.width / 3f, Screen.height / 1.5f);
		internal static Rect DragWindow = new Rect(0, 0, 10000, 20);
		internal static Rect CloseButton = new Rect(0, 0, 25, 15);

		private static int _currentHeight;
		private static int _currentWidth;

		private static bool _runOnce = true;

		internal static GUIStyle Separator;
		internal static GUIStyle MainWindow;
		internal static GUIStyle Sections;
		internal static GUIStyle Sections2;

		internal static GUIStyle ToggleLarge;

		private static Vector2 _scrollPosition;

		public static void DisplayGui()
		{
			//Setup some UI properties.
			if (_runOnce)
			{
				Separator = new GUIStyle(GUI.skin.horizontalSlider)
				{
					fixedHeight = 1f,
					normal =
					{
						background = UiToolbox.MakeTex(2, 2, new Color(0, 0, 0, 0.8f))
					},
					margin =
					{
						top = 10,
						bottom = 10
					}
				};

				MainWindow = new GUIStyle(GUI.skin.window)
				{
					normal =
					{
						background = UiToolbox.MakeWindowTex(new Color(0, 0, 0, 0.05f), new Color(0, 0, 0, 0.5f)),
						textColor = new Color(1, 1, 1, 0.05f)
					},
					hover =
					{
						background = UiToolbox.MakeWindowTex(new Color(0.3f, 0.3f, 0.3f, 0.3f), new Color(0, 0, 1, 0.5f)),
						textColor = new Color(1, 1, 1, 0.3f)
					},
					onNormal =
					{
						background = UiToolbox.MakeWindowTex(new Color(0.3f, 0.3f, 0.3f, 0.6f), new Color(0, 0, 1, 0.5f))
					}
				};

				Sections = new GUIStyle(GUI.skin.box)
				{
					normal =
					{
						background = UiToolbox.MakeTex(2, 2, new Color(0, 0, 0, 0.3f))
					}
				};

				Sections2 = new GUIStyle(GUI.skin.box)
				{
					normal =
					{
						background = UiToolbox.MakeTexWithRoundedCorner(new Color(0, 0, 0, 0.8f))
					}
				};

				ToggleLarge = new GUIStyle(GUI.skin.toggle);
				ToggleLarge.fontSize += 20;

				_runOnce = false;
			}

			//Sometimes the UI can be improperly sized, this sets it to some measurements.
			if (_currentHeight != Screen.height || _currentWidth != Screen.width)
			{
				WindowRect.height = Math.Max(Screen.height / 1.5f, 200);
				WindowRect.width = Math.Max(Screen.width / 3f, 500);

				WindowRect.y = Screen.height / 4f;
				WindowRect.x = Screen.width / 3f;

				AdvancedMaterialModifier.Logger.LogDebug($"Changing sizes of AdvancedMaterialModifier UI to {WindowRect.width} x {WindowRect.height}");

				_currentHeight = Screen.height;
				_currentWidth = Screen.width;
			}

			WindowRect = GUILayout.Window(WindowId, WindowRect, GuiWindowControls, "AdvancedMaterialModifier", MainWindow);
		}

		private static void GuiWindowControls(int windowId)
		{
			CloseButton.x = WindowRect.width - (CloseButton.width + 5);
			DragWindow.width = WindowRect.width - (CloseButton.width + 5);

			GUI.DragWindow(DragWindow);

			if (GUI.Button(CloseButton, "X"))
			{
				AdvancedMaterialModifier.EnabledGui = false;

				if (AdvancedMaterialModifier.Autosave.Value)
				{
					AdvancedMaterialModifier.SaveConfig();
				}
			}

			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

			foreach (var group in AdvancedMaterialModifier.Controls)
			{
				group.Value.DisplayUiElement();
			}

			GUILayout.EndScrollView();

			GUILayout.BeginHorizontal(Sections);

			if (GUILayout.Button("Export"))
			{
				AdvancedMaterialModifier.SaveConfig(true);
			}

			if (GUILayout.Button("Import"))
			{
				SorterSenderSaver.ModifyAll(true);
				AdvancedMaterialModifier.LoadConfig(true);
			}

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Reload"))
			{
				SorterSenderSaver.ModifyAll(true);
				AdvancedMaterialModifier.LoadConfig();
			}

			if (GUILayout.Button("Save"))
			{
				AdvancedMaterialModifier.SaveConfig();
			}

			GUILayout.EndHorizontal();

			UiToolbox.ChkMouseClick(WindowRect);
		}
	}
}