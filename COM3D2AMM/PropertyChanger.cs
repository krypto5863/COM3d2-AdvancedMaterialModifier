using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	//Concerned with taking materials and changing them to user spec.
	internal class PropertyChanger
	{
		private static readonly string RimPow = "_RimPower";
		private static readonly string NPRRimPow = "_RimLightPower";
		private static readonly string RimShift = "_RimShift";
		private static readonly string NPRRimShift = "_RimLightValue";
		private static readonly string RimCol = "_RimColor";
		private static readonly string NPRRimCol = "_RimLightColor";
		private static readonly string OutlineWid = "_OutlineWidth";
		private static readonly string OutlineCol = "_OutlineColor";
		private static readonly string Toon = "_ToonRamp";
		private static readonly string ShadowRate = "_ShadowRateToon";
		private static readonly string ShadowCol = "_ShadowColor";
		public static IEnumerator Run(Material material, CfgGroup cfg)
		{
#if (DEBUG)
			Debug.Log($"Editing material with name of: {material.name} with configuration for {cfg.GroupName}");
#endif

			yield return new WaitForEndOfFrame();

			if (material == null)
			{
				yield break;
			}
			//ModifyShaderIntoOutlineShader
			if (cfg.SetOutlineShader.Value)
			{
				if (!material.shader.name.Contains("Outline") && material.GetTag("Queue", true, null) != "Transparent" && material.GetTag("Queue", true, null) != "AlphaTest")
				{

				/*
					List<string> keywords = new List<string>(material.shaderKeywords);

					#if (DEBUG)
					foreach (string s in keywords){
						Debug.Log($"{material.shader.name} has the shader keyword of {s}");
					}
					#endif
					*/

					//if (!keywords.Contains("_ALPHAPREMULTIPLY_ON"))
					//{

						Shader shader = new Shader();

#if (DEBUG)
						Debug.Log($"Trying to fetch shader of name Shaders/" + material.shader.name + "_Outline");
#endif

						//shader = Resources.Load<Shader>("Shaders/" + material.shader.name + "_Outline");

						shader = Shader.Find(material.shader.name + "_Outline");

						if (shader == null)
						{
#if (DEBUG)
							Debug.Log($"No shader found, resorting to fallback!");
#endif
							//shader = Resources.Load<Shader>("Shaders/" + "CM3D2/Lighted_Outline");

							shader = Shader.Find("CM3D2/Toony_Lighted_Outline");
						}

						if (shader != null)
						{
#if (DEBUG)
							Debug.Log($"Setting {shader.name} as new material over {material.shader.name}");
#endif

							material.shader = shader;
						}
					//}
				}
				else if (material.shader.name.Contains("Outline_Tex"))
				{
					Shader shader = new Shader();

#if (DEBUG)
					Debug.Log($"Shaders/" + material.shader.name.Replace("_Tex", ""));
#endif

					//shader = Resources.Load<Shader>("Shaders/" + material.shader.name.Replace("_Tex", ""));

					shader = Shader.Find(material.shader.name.Replace("_Tex", ""));

					if (shader == null)
					{
#if (DEBUG)
						Debug.Log($"No shader found, resorting to fallback!");
#endif

						shader = Shader.Find("CM3D2/Toony_Lighted_Outline");
					}

					if (shader != null)
					{
#if (DEBUG)
						Debug.Log($"Setting {shader.name} as new material over {material.shader.name}");
#endif

						material.shader = shader;
					}
				}
			}

			//ModifyRimPower
			if (cfg.EditRimPower.Value && material.GetFloat(RimPow) != cfg.RimPower.Value)
			{

#if (DEBUG)
				Debug.Log($"Editing RimPow");
#endif
				material.SetFloat(RimPow, cfg.RimPower.Value);
			}
			if (cfg.EditRimPower.Value && material.GetFloat(NPRRimPow) != cfg.RimPower.Value)
			{
				material.SetFloat(NPRRimPow, cfg.RimPower.Value);
			}
			//ModifyRimShift
			if (cfg.EditRimShift.Value && material.GetFloat(RimShift) != cfg.RimShift.Value)
			{
#if (DEBUG)
				Debug.Log($"Editing RimShift");
#endif
				material.SetFloat(RimShift, cfg.RimShift.Value);
			}
			if (cfg.EditRimShift.Value && material.GetFloat(NPRRimShift) != cfg.RimShift.Value)
			{
#if (DEBUG)
				Debug.Log($"Editing RimShift");
#endif
				material.SetFloat(NPRRimShift, cfg.RimShift.Value);
			}
			//ModifyRimColor
			if (cfg.EditRimColor.Value)
			{
#if (DEBUG)
				Debug.Log($"Editing RimCol");
#endif
				Color c = new Color(cfg.RimRed.Value, cfg.RimGreen.Value, cfg.RimBlue.Value, cfg.RimAlpha.Value);

				if (material.GetColor(RimCol) != c)
				{
					material.SetColor(RimCol, c);
				}

				if (material.GetColor(NPRRimCol) != c)
				{
					material.SetColor(NPRRimCol, c);
				}
			}
			//ModifyOutlineWidth
			if (cfg.EditOutlineWidth.Value && material.GetFloat(OutlineWid) != cfg.OutlineWidth.Value)
			{
#if (DEBUG)
				Debug.Log($"Editing Outline Width");
#endif
				material.SetFloat(OutlineWid, cfg.OutlineWidth.Value);
			}
			//ModifyOutlineColor
			if (cfg.EditOutlineColor.Value)
			{
#if (DEBUG)
				Debug.Log($"Editing OutlineColor");
#endif
				Color c = new Color(cfg.OutlineRed.Value, cfg.OutlineGreen.Value, cfg.OutlineBlue.Value, cfg.OutlineAlpha.Value);

				if (material.GetColor(OutlineCol) != c)
				{
					material.SetColor(OutlineCol, c);
				}
			}
			//ModifyToonRamp
			if (cfg.ChangeToonTex.Value)
			{
				try
				{
					Texture2D tex = ImportCM.CreateTexture(cfg.ToonTex.Value);

					if (material.GetTexture(Toon) != tex)
					{
						material.SetTexture(Toon, tex);
					}
				}
				catch { };
			}
			//ModifyShadowRateToon
			if (cfg.ChangeShadowRateTex.Value)
			{
				try
				{
					Texture2D tex = ImportCM.CreateTexture(cfg.ShadowRateTex.Value);

					if (material.GetTexture(ShadowRate) != tex)
					{
						material.SetTexture(ShadowRate, tex);
					}
				}
				catch { };
			}
			//ModifyShadowColor
			if (cfg.EditShadowColor.Value)
			{
				Color c = new Color(cfg.ShadowRed.Value, cfg.ShadowGreen.Value, cfg.ShadowBlue.Value, cfg.ShadowAlpha.Value);

				if (material.GetColor(ShadowCol) != c)
				{
					material.SetColor(ShadowCol, c);
				}
			}
		}
		public static IEnumerator ChangeShadows(Renderer renderer, CfgGroup cfg)
		{
			if (cfg.EditShadowCasting.Value && (int)renderer.shadowCastingMode != cfg.ShadowCast.Value)
			{
				renderer.shadowCastingMode = (ShadowCastingMode)cfg.ShadowCast.Value;
			}

			if (cfg.EditShadowRecieving.Value && renderer.receiveShadows != cfg.ShadowRecieving.Value)
			{
				renderer.receiveShadows = cfg.ShadowRecieving.Value;
			}

			yield return null;
		}
	}
}
