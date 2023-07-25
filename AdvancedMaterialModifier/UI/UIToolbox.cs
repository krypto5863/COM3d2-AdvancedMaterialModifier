﻿using BepInEx;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AdvancedMaterialModifier.UI
{
	internal static class UiToolbox
	{
		internal static int LastMouseButtonUp { private set; get; } = -1;

		internal static void ChkMouseClick(Rect windowRect)
		{
			LastMouseButtonUp = Input.GetMouseButtonUp(0) ? 0 : Input.GetMouseButtonUp(1) ? 1 : Input.GetMouseButtonUp(2) ? 2 : -1;

			if ((Input.mouseScrollDelta.y != 0 || LastMouseButtonUp >= 0) && IsMouseOnGUI(windowRect))
			{
				Input.ResetInputAxes();
			}
		}

		internal static bool IsMouseOnGUI(Rect? windowRect = null)
		{
			if (windowRect.HasValue)
			{
				windowRect = MainGui.WindowRect;
			}

			var point = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			return AdvancedMaterialModifier.EnabledGui && windowRect.HasValue && windowRect.Value.Contains(point);
		}

		private static readonly Regex NotNumPeriod = new Regex("[^0-9.-]");

		internal static float FloatField(float initialVal, float min = 0, float max = 100)
		{
			var stringReturn = GUILayout.TextField(initialVal.ToString("0.0#####"), GUILayout.Width(75));
			stringReturn = NotNumPeriod.Replace(stringReturn, "");
			stringReturn = stringReturn.IsNullOrWhiteSpace() ? "0" : stringReturn;

			return float.TryParse(stringReturn, out var floatReturn) ? floatReturn : initialVal;
		}

		internal static float HorizontalSliderWithInputBox(float initialVal, float min = 0, float max = 100, string label = null, bool doButtons = true)
		{
			GUILayout.BeginHorizontal();

			if (label.IsNullOrWhiteSpace() == false)
			{
				GUILayout.Label(label);
			}

			initialVal = GUILayout.HorizontalSlider(initialVal, min, max, GUILayout.MaxWidth(9999));

			if (doButtons)
			{
				GUILayout.BeginHorizontal();

				GUILayout.FlexibleSpace();

				if (GUILayout.Button("<"))
				{
					initialVal -= (max * 0.01f);
				}

				if (GUILayout.Button("0"))
				{
					initialVal = 0;
				}

				if (GUILayout.Button(">"))
				{
					initialVal += (max * 0.01f);
				}

				GUILayout.EndHorizontal();
			}

			initialVal = FloatField(initialVal, min, max);
			GUILayout.EndHorizontal();

			return initialVal;
		}

		internal static Texture2D MakeTex(int width, int height, Color col)
		{
			var pix = new Color[width * height];
			for (int i = 0; i < pix.Length; ++i)
			{
				pix[i] = col;
			}
			var result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();
			return result;
		}

		internal static Texture2D MakeTexWithRoundedCorner(Color col)
		{
			var xy = 12;
			var result = new Texture2D(xy, xy);
			for (var i = 0; i < xy; ++i)
			{
				for (var j = 0; j < xy; j++)
				{
					var topLeft = (i == 0 && (j == 0 || j == 1)) || (j == 0 && (i == 0 || i == 1));
					var bottomLeft = (i == 0 && (j == (xy - 1) || j == (xy - 2))) || (j == (xy - 1) && (i == 0 || i == 1));
					var topRight = (i == (xy - 1) && (j == 0 || j == 1)) || (j == 0 && (i == (xy - 1) || i == (xy - 2)));
					var bottomRight = (i == (xy - 1) && (j == (xy - 1) || j == (xy - 2))) || (j == (xy - 1) && (i == (xy - 1) || i == (xy - 2)));

					//Corner
					if (topLeft || topRight || bottomLeft || bottomRight)
					{
						result.SetPixels(i, j, 1, 1, new[] { new Color(0, 0, 0, 0) });
					}
					//Border
					else if (i == 0 || j == 0 || i == (xy - 1) || j == (xy - 1) ||
							(i == 1 && j == 1) || (i == (xy - 2) && j == 1) || (i == 1 && j == (xy - 2)) || (i == (xy - 2) && j == (xy - 2)))
					{
						result.SetPixels(i, j, 1, 1, new[] { Color.black });
					}
					//Normal
					else
					{
						result.SetPixels(i, j, 1, 1, new[] { col });
					}
				}
			}

			result.Apply();
			return result;
		}

		internal class P
		{
			public int X { get; set; }
			public int Y { get; set; }

			public P(int x, int y)
			{
				X = x;
				Y = y;
			}
		}

		internal static Texture2D MakeWindowTex(Color col, Color col2)
		{
			int x = 17;
			int y = 27;
			P[] nulls = { new P(0,0), new P(0,1), new P(0,2), new P(0,3), new P(0,4),
								  new P(1,0), new P(1,1), new P(1,2), new P(1,3),
								  new P(2,0), new P(2,1), new P(2,2),
								  new P(3,0), new P(3,1),
								  new P(4,0),
								  new P(x-1-0,0), new P(x-1-0,1), new P(x-1-0,2), new P(x-1-0,3), new P(x-1-0,4),
								  new P(x-1-1,0), new P(x-1-1,1), new P(x-1-1,2), new P(x-1-1,3),
								  new P(x-1-2,0), new P(x-1-2,1), new P(x-1-2,2),
								  new P(x-1-3,0), new P(x-1-3,1),
								  new P(x-1-4,0),
								  new P(0,y-1-0), new P(0,y-1-1), new P(0,y-1-2), new P(0,y-1-3), new P(0,y-1-4),
								  new P(1,y-1-0), new P(1,y-1-1), new P(1,y-1-2), new P(1,y-1-3),
								  new P(2,y-1-0), new P(2,y-1-1), new P(2,y-1-2),
								  new P(3,y-1-0), new P(3,y-1-1),
								  new P(4,y-1-0),
								  new P(x-1-0,y-1-0), new P(x-1-0,y-1-1), new P(x-1-0,y-1-2), new P(x-1-0,y-1-3), new P(x-1-0,y-1-4),
								  new P(x-1-1,y-1-0), new P(x-1-1,y-1-1), new P(x-1-1,y-1-2), new P(x-1-1,y-1-3),
								  new P(x-1-2,y-1-0), new P(x-1-2,y-1-1), new P(x-1-2,y-1-2),
								  new P(x-1-3,y-1-0), new P(x-1-3,y-1-1),
								  new P(x-1-4,y-1-0)};
			P[] brdrS = { new P(4,1), new P(3,2), new P(2,3), new P(1, 4),
								  new P(x-1-4,1), new P(x-1-3,2), new P(x-1-2,3), new P(x-1-1, 4),
								  new P(4,y-1-1), new P(3,y-1-2), new P(2,y-1-3), new P(1, y-1-4),
								  new P(x-1-4,y-1-1), new P(x-1-3,y-1-2), new P(x-1-2,y-1-3), new P(x-1-1, y-1-4)};

			Texture2D result = new Texture2D(x, y);
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					//Border
					if (i == 0 || j == 0 || i == (x - 1) || j == (y - 1) || brdrS.ToList().Exists(p => p.X == i && p.Y == j))
					{
						result.SetPixels(i, j, 1, 1, new[] { Color.black });
					}
					else
					{
						result.SetPixels(i, j, 1, 1, j <= 10 ? new[] { col } : new[] { col2 });
					}

					//Corner
					if (nulls.ToList().Exists(p => p.X == i && p.Y == j))
					{
						result.SetPixels(i, j, 1, 1, new[] { new Color(0, 0, 0, 0) });
					}
				}
			}

			result.Apply();
			return result;
		}
	}
}