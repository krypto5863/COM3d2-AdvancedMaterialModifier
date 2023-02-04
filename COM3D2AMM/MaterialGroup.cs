using COM3D2.AdvancedMaterialModifier.UI;
using Newtonsoft.Json;
using System.ComponentModel;
using UnityEngine;

namespace COM3D2.AdvancedMaterialModifier
{
	public class MaterialGroup : INotifyPropertyChanged
	{
		[JsonProperty]
		public bool Enable { get; private set; }

		[JsonProperty]
		public bool EditColor { get; private set; }

		[JsonProperty]
		public float Red { get; private set; } = 1f;

		[JsonProperty]
		public float Green { get; private set; } = 1f;

		[JsonProperty]
		public float Blue { get; private set; } = 1f;

		[JsonProperty]
		public bool EditShininess { get; private set; }

		[JsonProperty]
		public float Shininess { get; private set; }

		[JsonProperty]
		public bool EditRimPower { get; private set; }

		[JsonProperty]
		public float RimPower { get; private set; }

		[JsonProperty]
		public bool EditRimShift { get; private set; }

		[JsonProperty]
		public float RimShift { get; private set; }

		[JsonProperty]
		public bool EditRimColor { get; private set; }

		//[JsonProperty]
		//public float RimAlpha { get; private set; }

		[JsonProperty]
		public float RimRed { get; private set; }

		[JsonProperty]
		public float RimGreen { get; private set; }

		[JsonProperty]
		public float RimBlue { get; private set; }

		[JsonProperty]
		public bool EditOutlineWidth { get; private set; }

		[JsonProperty]
		public float OutlineWidth { get; private set; }

		[JsonProperty]
		public bool SetOutlineShader { get; private set; }

		[JsonProperty]
		public bool EditOutlineColor { get; private set; }

		//[JsonProperty]
		//public float OutlineAlpha { get; private set; }

		[JsonProperty]
		public float OutlineRed { get; private set; }

		[JsonProperty]
		public float OutlineGreen { get; private set; }

		[JsonProperty]
		public float OutlineBlue { get; private set; }

		[JsonProperty]
		public bool ChangeToonTex { get; private set; }

		[JsonProperty]
		public string ToonTex { get; private set; } = string.Empty;

		[JsonProperty]
		public bool ChangeShadowRateTex { get; private set; }

		[JsonProperty]
		public string ShadowRateTex { get; private set; } = string.Empty;

		[JsonProperty]
		public bool EditShadowCasting { get; private set; }

		[JsonProperty]
		public int ShadowCast { get; private set; }

		[JsonProperty]
		public bool EditShadowReceiving { get; private set; }

		[JsonProperty]
		public bool ShadowReceiving { get; private set; }

		[JsonProperty]
		public bool EditShadowColor { get; private set; }

		//[JsonProperty]
		//public float ShadowAlpha { get; private set; }

		[JsonProperty]
		public float ShadowRed { get; private set; }

		[JsonProperty]
		public float ShadowGreen { get; private set; }

		[JsonProperty]
		public float ShadowBlue { get; private set; }

		[JsonProperty]
		public string GroupName { get; private set; }

		private bool _expanded;

		public event PropertyChangedEventHandler PropertyChanged;

		public MaterialGroup(string name)
		{
			GroupName = name;
			PropertyChanged += (s, e) =>
			{
#if (DEBUG)
				AMM.Logger.LogMessage("Property changed, sending coroute.");
#endif
				SorterSenderSaver.ModifyGroup(this);
			};
		}

		public void DisplayUiElement()
		{
			GUILayout.BeginVertical(MainGui.Sections);

			GUILayout.BeginHorizontal(MainGui.Sections2);
			Enable = GUILayout.Toggle(Enable, GroupName, MainGui.ToggleLarge);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("☰"))
			{
				_expanded = !_expanded;
			}
			GUILayout.EndHorizontal();

			if (_expanded)
			{
				GUILayout.BeginVertical(MainGui.Sections);
				EditColor = GUILayout.Toggle(EditColor, "Color");

				//RimAlpha = UIToolbox.HorizontalSliderWithInputBox(RimAlpha, -1, 1, "A");
				Red = UiToolbox.HorizontalSliderWithInputBox(Red, -1, 1, "R");
				Green = UiToolbox.HorizontalSliderWithInputBox(Green, -1, 1, "G");
				Blue = UiToolbox.HorizontalSliderWithInputBox(Blue, -1, 1, "B");
				GUILayout.EndHorizontal();

				GUILayout.BeginVertical(MainGui.Sections);
				EditShininess = GUILayout.Toggle(EditShininess, "Shininess");
				Shininess = UiToolbox.HorizontalSliderWithInputBox(Shininess, -25, 25);
				GUILayout.EndHorizontal();

				GUILayout.BeginVertical(MainGui.Sections);
				EditRimPower = GUILayout.Toggle(EditRimPower, "RimPower");
				RimPower = UiToolbox.HorizontalSliderWithInputBox(RimPower, -200, 200);
				GUILayout.EndHorizontal();

				GUILayout.BeginVertical(MainGui.Sections);
				EditRimShift = GUILayout.Toggle(EditRimShift, "RimShift");
				RimShift = UiToolbox.HorizontalSliderWithInputBox(RimShift, -4, 4);
				GUILayout.EndHorizontal();

				GUILayout.BeginVertical(MainGui.Sections);

				EditRimColor = GUILayout.Toggle(EditRimColor, "RimLighting Color");

				//RimAlpha = UIToolbox.HorizontalSliderWithInputBox(RimAlpha, -1, 1, "A");
				RimRed = UiToolbox.HorizontalSliderWithInputBox(RimRed, -1, 1, "R");
				RimGreen = UiToolbox.HorizontalSliderWithInputBox(RimGreen, -1, 1, "G");
				RimBlue = UiToolbox.HorizontalSliderWithInputBox(RimBlue, -1, 1, "B");

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				EditOutlineWidth = GUILayout.Toggle(EditOutlineWidth, "Outline Width");
				OutlineWidth = UiToolbox.HorizontalSliderWithInputBox(OutlineWidth, 0, 0.1f);

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				SetOutlineShader = GUILayout.Toggle(SetOutlineShader, "Enforce Color-able Outline Shader");

				EditOutlineColor = GUILayout.Toggle(EditOutlineColor, "Outline Color");

				//OutlineAlpha = UIToolbox.HorizontalSliderWithInputBox(OutlineAlpha, -1, 1, "A");
				OutlineRed = UiToolbox.HorizontalSliderWithInputBox(OutlineRed, -1, 1, "R");
				OutlineGreen = UiToolbox.HorizontalSliderWithInputBox(OutlineGreen, -1, 1, "G");
				OutlineBlue = UiToolbox.HorizontalSliderWithInputBox(OutlineBlue, -1, 1, "B");

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				ChangeToonTex = GUILayout.Toggle(ChangeToonTex, "Toon Ramp");
				ToonTex = GUILayout.TextField(ToonTex);

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				ChangeShadowRateTex = GUILayout.Toggle(ChangeShadowRateTex, "Shadow Toon Ramp");

				ShadowRateTex = GUILayout.TextField(ShadowRateTex);

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				EditShadowCasting = GUILayout.Toggle(EditShadowCasting, "Shadow Casting");
				GUILayout.Label("0: Disabled. 1: Enabled. 2: Two Sided. 3: Shadows Only");
				ShadowCast = (int)UiToolbox.HorizontalSliderWithInputBox(ShadowCast, 0, 3, doButtons: false);

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				EditShadowReceiving = GUILayout.Toggle(EditShadowReceiving, "Shadow Receiving");
				ShadowReceiving = GUILayout.Toggle(ShadowReceiving, "Receive");

				GUILayout.EndVertical();

				GUILayout.BeginVertical(MainGui.Sections);

				EditShadowColor = GUILayout.Toggle(EditShadowColor, "Shadow Color");
				//ShadowAlpha = UIToolbox.HorizontalSliderWithInputBox(ShadowAlpha, -1, 1, "A");
				ShadowRed = UiToolbox.HorizontalSliderWithInputBox(ShadowRed, -1, 1, "R");
				ShadowGreen = UiToolbox.HorizontalSliderWithInputBox(ShadowGreen, -1, 1, "G");
				ShadowBlue = UiToolbox.HorizontalSliderWithInputBox(ShadowBlue, -1, 1, "B");

				GUILayout.EndVertical();
			}

			GUILayout.EndVertical();
		}
	}
}