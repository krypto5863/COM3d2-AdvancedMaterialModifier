using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace AdvancedMaterialModifier
{
	//Concerned with taking materials and changing them to user spec.
	internal static class PropertyChanger
	{
		private const string Color = "_Color";
		private const string Shininess = "_Shininess";
		private const string RimPow = "_RimPower";
		private const string NprRimPow = "_RimLightPower";
		private const string RimShift = "_RimShift";
		private const string NprRimShift = "_RimLightValue";
		private const string RimCol = "_RimColor";
		private const string NprRimCol = "_RimLightColor";
		private const string OutlineWid = "_OutlineWidth";
		private const string OutlineCol = "_OutlineColor";
		private const string Toon = "_ToonRamp";
		private const string ShadowRate = "_ShadowRateToon";
		private const string ShadowCol = "_ShadowColor";

		private static readonly Shader ToonyLightedOutline = Shader.Find("CM3D2/Toony_Lighted_Outline");
		private static Color _tempColor = new Color(0, 0, 0, 1);

		public static IEnumerator Run(Material material, MaterialGroup cfg)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Editing material with name of: {material.name} with configuration for {cfg.GroupName}");
#endif
			var isGlobal = AdvancedMaterialModifier.Controls["global"] == cfg;

			if (isGlobal == false)
			{
				yield return new WaitForEndOfFrame();
				yield return AdvancedMaterialModifier.This.StartCoroutine(Run(material, AdvancedMaterialModifier.Controls["global"]));
			}

			if (material == null || !cfg.Enable)
			{
				yield break;
			}

			//Modifies the shader if possible into an outline shader.
			if (cfg.SetOutlineShader)
			{
				if (!material.shader.name.Contains("Outline") && material.GetTag("Queue", true, null) != "Transparent" && material.GetTag("Queue", true, null) != "AlphaTest")
				{
					material.shader = Shader.Find(material.shader.name + "_Outline") ?? ToonyLightedOutline ?? material.shader;
				}
				else if (material.shader.name.Contains("Outline_Tex"))
				{
					material.shader = Shader.Find(material.shader.name.Replace("_Tex", string.Empty)) ?? ToonyLightedOutline ?? material.shader;
				}
			}

			if (cfg.EditColor)
			{
				_tempColor.r = cfg.Red;
				_tempColor.g = cfg.Green;
				_tempColor.b = cfg.Blue;
				_tempColor.a = material.GetColor(Color).a;

				material.SetColor(Color, _tempColor);
			}

			//modifies Shininess
			if (cfg.EditShininess)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing Shininess");
#endif
				material.SetFloat(Shininess, cfg.Shininess);
			}

			//modifies rim power
			if (cfg.EditRimPower)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing RimPow");
#endif
				if (material.GetFloat(RimPow) != cfg.RimPower)
				{
					material.SetFloat(RimPow, cfg.RimPower);
				}

				if (material.GetFloat(NprRimPow) != cfg.RimPower)
				{
					material.SetFloat(NprRimPow, cfg.RimPower);
				}
			}

			//Modify rim shift
			if (cfg.EditRimShift)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing RimShift");
#endif
				if (material.GetFloat(RimShift) != cfg.RimShift)
				{
					material.SetFloat(RimShift, cfg.RimShift);
				}

				if (material.GetFloat(NprRimShift) != cfg.RimShift)
				{
					material.SetFloat(NprRimShift, cfg.RimShift);
				}
			}

			//Modify rim color
			if (cfg.EditRimColor)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing RimCol");
#endif
				//var c = new Color(cfg.RimRed, cfg.RimGreen, cfg.RimBlue, cfg.RimAlpha);
				_tempColor.r = cfg.RimRed;
				_tempColor.g = cfg.RimGreen;
				_tempColor.b = cfg.RimBlue;
				//tempColor.a = cfg.RimAlpha;

				if (material.GetColor(RimCol) != _tempColor)
				{
					material.SetColor(RimCol, _tempColor);
				}

				if (material.GetColor(NprRimCol) != _tempColor)
				{
					material.SetColor(NprRimCol, _tempColor);
				}
			}
			//Modify outline's width
			if (cfg.EditOutlineWidth && material.GetFloat(OutlineWid) != cfg.OutlineWidth)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing Outline Width");
#endif
				material.SetFloat(OutlineWid, cfg.OutlineWidth);
			}
			//Modify Outline Color
			if (cfg.EditOutlineColor)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Editing OutlineColor");
#endif
				//Color c = new Color(cfg.OutlineRed, cfg.OutlineGreen, cfg.OutlineBlue, cfg.OutlineAlpha);
				_tempColor.r = cfg.OutlineRed;
				_tempColor.g = cfg.OutlineGreen;
				_tempColor.b = cfg.OutlineBlue;
				//tempColor.a = cfg.OutlineAlpha;

				if (material.GetColor(OutlineCol) != _tempColor)
				{
					material.SetColor(OutlineCol, _tempColor);
				}
			}
			//Modify Toon Ramp
			if (cfg.ChangeToonTex)
			{
				try
				{
					Texture2D tex = ImportCM.CreateTexture(cfg.ToonTex);

					if (material.GetTexture(Toon) != tex)
					{
						material.SetTexture(Toon, tex);
					}
				}
				catch
				{
					// ignored
				}
			}
			//Modify Shadow Rate Toon
			if (cfg.ChangeShadowRateTex)
			{
				try
				{
					Texture2D tex = ImportCM.CreateTexture(cfg.ShadowRateTex);

					if (material.GetTexture(ShadowRate) != tex)
					{
						material.SetTexture(ShadowRate, tex);
					}
				}
				catch
				{
					//Ignored
				}
			}
			//Modify Shadow Color
			if (cfg.EditShadowColor)
			{
				//Color c = new Color(cfg.ShadowRed, cfg.ShadowGreen, cfg.ShadowBlue, cfg.ShadowAlpha);
				_tempColor.r = cfg.ShadowRed;
				_tempColor.g = cfg.ShadowGreen;
				_tempColor.b = cfg.ShadowBlue;
				//tempColor.a = cfg.ShadowAlpha;

				if (material.GetColor(ShadowCol) != _tempColor)
				{
					material.SetColor(ShadowCol, _tempColor);
				}
			}
		}

		internal static IEnumerator ChangeShadows(Renderer renderer, MaterialGroup cfg)
		{
			var isGlobal = AdvancedMaterialModifier.Controls["global"] == cfg;
			if (isGlobal == false)
			{
				yield return new WaitForEndOfFrame();
				yield return AdvancedMaterialModifier.This.StartCoroutine(ChangeShadows(renderer, AdvancedMaterialModifier.Controls["global"]));
			}

			renderer.shadowCastingMode = cfg.EditShadowCasting ? (ShadowCastingMode)cfg.ShadowCast : renderer.shadowCastingMode;
			renderer.receiveShadows = cfg.EditShadowReceiving ? cfg.ShadowReceiving : renderer.receiveShadows;
		}
	}
}