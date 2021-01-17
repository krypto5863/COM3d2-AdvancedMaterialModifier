using BepInEx.Configuration;
using System;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	public class CfgGroup
	{
		public ConfigEntry<bool> Enable { get; set; }

		public ConfigEntry<bool> EditRimPower { get; set; }
		public ConfigEntry<float> RimPower { get; set; }
		public ConfigEntry<bool> EditRimShift { get; set; }
		public ConfigEntry<float> RimShift { get; set; }
		public ConfigEntry<bool> EditRimColor { get; set; }
		public ConfigEntry<float> RimAlpha { get; set; }
		public ConfigEntry<float> RimRed { get; set; }
		public ConfigEntry<float> RimGreen { get; set; }
		public ConfigEntry<float> RimBlue { get; set; }

		public ConfigEntry<bool> EditOutlineWidth { get; set; }
		public ConfigEntry<float> OutlineWidth { get; set; }

		public ConfigEntry<bool> SetOutlineShader { get; set; }

		public ConfigEntry<bool> EditOutlineColor { get; set; }
		public ConfigEntry<float> OutlineAlpha { get; set; }
		public ConfigEntry<float> OutlineRed { get; set; }
		public ConfigEntry<float> OutlineGreen { get; set; }
		public ConfigEntry<float> OutlineBlue { get; set; }

		public ConfigEntry<bool> ChangeToonTex { get; set; }
		public ConfigEntry<string> ToonTex { get; set; }

		public ConfigEntry<bool> ChangeShadowRateTex { get; set; }
		public ConfigEntry<string> ShadowRateTex { get; set; }

		public ConfigEntry<bool> EditShadowCasting { get; set; }

		public ConfigEntry<int> ShadowCast { get; set; }

		public ConfigEntry<bool> EditShadowRecieving { get; set; }

		public ConfigEntry<bool> ShadowRecieving { get; set; }

		public ConfigEntry<bool> EditShadowColor { get; set; }

		public ConfigEntry<float> ShadowAlpha { get; set; }
		public ConfigEntry<float> ShadowRed { get; set; }
		public ConfigEntry<float> ShadowGreen { get; set; }
		public ConfigEntry<float> ShadowBlue { get; set; }

		public string GroupName { get; set; }

		public event EventHandler SettingChanged;

		public CfgGroup(string name)
		{
			this.GroupName = name;

			Enable = Init.@this.Config.Bind(name, "0: Enable", false, "DeRim will affect the Slot.");

			EditRimPower = Init.@this.Config.Bind(name, "1: Change RimPower", false, "DeRim will affect the Slot's RimPower setting.");
			RimPower = Init.@this.Config.Bind(name, "2: RimPower", float.Parse("0"), "RimPower is a rim lighting setting that denotes how far into the center of the model from the viewpoint the rimlighting can go.");

			EditRimShift = Init.@this.Config.Bind(name, "3: Change RimShift", false, "DeRim will affect the Slot's RimShift setting.");
			RimShift = Init.@this.Config.Bind(name, "4: RimShift", float.Parse("0"), "RimShift is a rim lighting setting that denotes how far into the falloff of the rim lighting.");

			EditRimColor = Init.@this.Config.Bind(name, "5: Change RimLighting Color", false, "DeRim will affect the Slot's RimColor setting.");

			RimAlpha = Init.@this.Config.Bind(name, "6: Alpha", float.Parse("0"), "RimLightings Alpha Color Property");
			RimRed = Init.@this.Config.Bind(name, "7: Red", float.Parse("0"), "RimLightings Red Color Property");
			RimGreen = Init.@this.Config.Bind(name, "8: Green", float.Parse("0"), "RimLightings Green Color Property");
			RimBlue = Init.@this.Config.Bind(name, "9: Blue", float.Parse("0"), "RimLightings Blue Color Property");

			EditOutlineWidth = Init.@this.Config.Bind(name, "A: Change Outline Width", false, "DeRim will have an effect on the outline.");

			OutlineWidth = Init.@this.Config.Bind(name, "B: Outline Width", float.Parse("0"), "Allows you to change the width of the outline.");

			SetOutlineShader = Init.@this.Config.Bind(name, "C: Enforce Colorable Outline Shaders", false, "Forces a material to use a shader that allows outlines and the changing of the color of outlines.");

			EditOutlineColor = Init.@this.Config.Bind(name, "D: Change Outline Color", false, "DeRim will change the outline color. Do note, this is completely ignored by certain shader types.");

			OutlineAlpha = Init.@this.Config.Bind(name, "E: Alpha", float.Parse("0"), "Outline's Alpha Color Property");
			OutlineRed = Init.@this.Config.Bind(name, "G: Red", float.Parse("0"), "Outline's Red Color Property");
			OutlineGreen = Init.@this.Config.Bind(name, "G: Green", float.Parse("0"), "Outline's Green Color Property");
			OutlineBlue = Init.@this.Config.Bind(name, "H: Blue", float.Parse("0"), "Outline's Blue Color Property");

			ChangeToonTex = Init.@this.Config.Bind(name, "I: Change Toon Ramp", false, "Changes the toon texture for the Slot.");
			ToonTex = Init.@this.Config.Bind(name, "J: Toon Ramp", "", "Example: toonreda1.tex");

			ChangeShadowRateTex = Init.@this.Config.Bind(name, "K: Change Shadow Toon Ramp", false, "Changes the shadow toon texture for the Slot");
			ShadowRateTex = Init.@this.Config.Bind(name, "L: Shadow Toon Ramp", "", "Example: toonreda1.tex");

			EditShadowCasting = Init.@this.Config.Bind(name, "M: Change Shadow Casting", false, "Derim will enforce your shadow casting setting on all items in this group.");

			ShadowCast = Init.@this.Config.Bind(name, "N: Cast Shadow", 0, "0: Disabled. 1: Enabled. 2: Two Sided. 3: Shadows Only");

			EditShadowRecieving = Init.@this.Config.Bind(name, "O: Change Shadow Recieving", false, "Derim will enforce your shadow recieving setting on all items in this group.");

			ShadowRecieving = Init.@this.Config.Bind(name, "P: Shadow Recieving", true, "Enabled means item recieves shadows. Disabled means it does not.");

			EditShadowColor = Init.@this.Config.Bind(name, "Q: Change Shadow Color", false, "DeRim will change the outline color. Do note, this is completely ignored by certain shader types.");

			ShadowAlpha = Init.@this.Config.Bind(name, "R: Alpha", float.Parse("0"), "Shadow's Alpha Color Property");
			ShadowRed = Init.@this.Config.Bind(name, "S: Red", float.Parse("0"), "Shadow's Red Color Property");
			ShadowGreen = Init.@this.Config.Bind(name, "T: Green", float.Parse("0"), "Shadow's Green Color Property");
			ShadowBlue = Init.@this.Config.Bind(name, "U: Blue", float.Parse("0"), "Shadow's Blue Color Property");

			Enable.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditRimPower.SettingChanged += (s, e) => { SettingChanged(s, e); };
			EditRimPower.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditRimShift.SettingChanged += (s, e) => { SettingChanged(s, e); };
			RimShift.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditRimColor.SettingChanged += (s, e) => { SettingChanged(s, e); };
			RimAlpha.SettingChanged += (s, e) => { SettingChanged(s, e); };
			RimRed.SettingChanged += (s, e) => { SettingChanged(s, e); };
			RimGreen.SettingChanged += (s, e) => { SettingChanged(s, e); };
			RimBlue.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditOutlineWidth.SettingChanged += (s, e) => { SettingChanged(s, e); };
			OutlineWidth.SettingChanged += (s, e) => { SettingChanged(s, e); };

			SetOutlineShader.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditOutlineColor.SettingChanged += (s, e) => { SettingChanged(s, e); };
			OutlineAlpha.SettingChanged += (s, e) => { SettingChanged(s, e); };
			OutlineRed.SettingChanged += (s, e) => { SettingChanged(s, e); };
			OutlineGreen.SettingChanged += (s, e) => { SettingChanged(s, e); };
			OutlineBlue.SettingChanged += (s, e) => { SettingChanged(s, e); };

			ChangeToonTex.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ToonTex.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditShadowCasting.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowCast.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditShadowRecieving.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowRecieving.SettingChanged += (s, e) => { SettingChanged(s, e); };

			ChangeShadowRateTex.SettingChanged += (s, e) => { SettingChanged(s, e); };

			ChangeShadowRateTex.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowRateTex.SettingChanged += (s, e) => { SettingChanged(s, e); };

			EditShadowColor.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowAlpha.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowRed.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowGreen.SettingChanged += (s, e) => { SettingChanged(s, e); };
			ShadowBlue.SettingChanged += (s, e) => { SettingChanged(s, e); };
		}
	}
}
