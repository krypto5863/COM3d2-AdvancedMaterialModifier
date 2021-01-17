using DeRim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3d2.DeRim.Plugin
{
	class ModifyMaterial
	{
		public static void ModMaterial(Material material, float OutWVal, float RimSVal, float RimPVal, bool modrim, Color RimCol, Color OutCol, string toon, string stoon)
		{
#if (DEBUG)
			Debug.Log("Started material modification");
#endif

			const string OUTWIDTH = "_OutlineWidth";
			const string RIMCOLOR = "_RimColor";
			const string NPRRIMCOLOR = "_RimLightColor";
			const string RIMPOWER = "_RimPower";
			const string RIMSHIFT = "_RimShift";
			//Color RColor; RColor.a = 0; RColor.r = 0; RColor.g = 0; RColor.b = 0;
			//Color OColor; OColor.a = 1; OColor.r = 1; OColor.g = 1; OColor.b = 1;

			//Console.WriteLine("Alpha color found was: " + material.GetColor(RIMCOLOR).a);
			// Console.WriteLine("Desired Alpha value is: " + RimAVal);



				if (Main.ToonMod.Value && material.GetTexture("_ToonRamp"))
				{
					try
					{
						Texture2D ntoon = ImportCM.CreateTexture(stoon);

						Console.WriteLine(material.GetTexture("_ToonRamp").name);

						material.SetTexture("_ToonRamp", ntoon);
					}
					catch
					{

					}
				}

				if (Main.SToonMod.Value && material.GetTexture("_ShadowRateToon"))
				{
					try
					{
						Texture2D nstoon = ImportCM.CreateTexture(stoon);

						Console.WriteLine(material.GetTexture("_ShadowRateToon").name);

						material.SetTexture("_ShadowRateToon", nstoon);
					}
					catch
					{

					}
				}

			if (Main.RimMod.Value == true) {

				if (material.GetFloat(RIMPOWER) != RimPVal)
				{
#if (DEBUG)
					Console.WriteLine("RimPower was found equal to: " + material.GetFloat(RIMPOWER));
					Console.WriteLine("Setting rimpower to: " + RimPVal);
#endif
					material.SetFloat(RIMPOWER, RimPVal);
				}

				if (material.GetFloat(RIMSHIFT) != RimSVal)
				{
#if (DEBUG)
					Console.WriteLine("RimShift was found equal to: " + material.GetFloat(RIMSHIFT));
					Console.WriteLine("Setting rimshift to: " + RimSVal);
#endif
					material.SetFloat(RIMSHIFT, RimSVal);
				}

				if (material.GetColor(RIMCOLOR) != RimCol)
				{
#if (DEBUG)
					Console.WriteLine("Found Rimcolor not equal to desired. Changing!");
#endif
					material.SetColor(RIMCOLOR, RimCol);
				}

				if (material.GetColor(NPRRIMCOLOR) != RimCol)
				{
#if (DEBUG)
					Console.WriteLine("Found NPR Rimcolor not equal to desired. Changing!");
#endif
					material.SetColor(NPRRIMCOLOR, RimCol);
				}
			}

			if (Main.OutMod.Value == true)
			{
				if (material.GetColor("_OutlineColor") != OutCol)
				{
#if (DEBUG)
					Console.WriteLine("Found OutlineColor not equal to desired. Changing!");
#endif
					material.SetColor("_OutlineColor", OutCol);
				}

				if (material.GetFloat(OUTWIDTH) != OutWVal)
				{
#if (DEBUG)
					Console.WriteLine("Outline was found equal to: " + material.GetFloat(OUTWIDTH));
					Console.WriteLine("Setting outline to: " + OutWVal);
#endif
					material.SetFloat(OUTWIDTH, OutWVal);
				}
			}
		}
	}
}
