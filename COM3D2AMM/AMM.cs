using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	[BepInPlugin("AdvancedMaterialModifier", "AdvancedMaterialModifier", "1.0")]
	[BepInDependency("org.bepinex.plugins.unityinjectorloader", BepInDependency.DependencyFlags.SoftDependency)]
	internal class Init : BaseUnityPlugin
	{

		public static CfgGroup Body;
		public static CfgGroup Head;
		public static CfgGroup Hair;
		public static CfgGroup Clothes;
		public static CfgGroup Acc;
		public static CfgGroup Bg;
		public static CfgGroup Props;
		public static CfgGroup MpsProps;

		public static Init @this;

		//concerned with setting the playing field for DeRim to operate.
		private void Awake()
		{
			DontDestroyOnLoad(this);

			@this = this;

			Body = new CfgGroup("0: Body");
			Head = new CfgGroup("1: Head");
			Hair = new CfgGroup("2: Hair");
			Clothes = new CfgGroup("3: Clothes");
			Acc = new CfgGroup("4: Acc");
			Bg = new CfgGroup("5: Backgrounds");
			Props = new CfgGroup("6: Props");
			MpsProps = new CfgGroup("7: MPS Loaded Props");

			Harmony.CreateAndPatchAll(typeof(VanillaHarmonyPatch));

			Body.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Body)); };
			Head.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Head)); };
			Hair.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Hair)); };
			Clothes.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Clothes)); };
			Acc.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Acc)); };
			Props.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Props)); };
			Bg.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(Bg)); };
#if (DEBUG)
			Debug.Log("Starting MPS patching support!");
#endif
			try
			{
				Harmony.CreateAndPatchAll(typeof(MPSHarmonyPatch));
				MpsProps.SettingChanged += (s, e) => { StartCoroutine(SorterSenderSaver.ModifyAllOfGroup(MpsProps)); };
			}
			catch
			{
				Debug.LogWarning("MPS could not be patched! Likely not loaded.");
			}
			Console.WriteLine("Declaring war on Rim Lighting!!");
		}
	}
}