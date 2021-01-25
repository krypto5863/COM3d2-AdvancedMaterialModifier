using HarmonyLib;
using System.Linq;
using UnityEngine;
using static COM3D2.AdvancedMaterialModifier.Plugin.Init;
using static COM3D2.AdvancedMaterialModifier.Plugin.SorterSenderSaver;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	internal class VanillaHarmonyPatch
	{
		//This one captures the loading of maids and maid items.
		[HarmonyPatch(typeof(ImportCM), "LoadSkinMesh_R")]
		[HarmonyPostfix]
		private static void NotifyOfLoad(ref string __2, ref GameObject __result)
		{
#if (DEBUG)
			Debug.Log("Picked up a mesh change");
#endif
			int slotid = (int)TBody.hashSlotName[__2];

			AddToObjectDictionary(__result, slotid);

			@this.StartCoroutine(ModifySingle(__result));
		}
		//This function is concerned with tracking material edits and alerting AMM so it can correct it.
		[HarmonyPatch(typeof(TBody), "ChangeMaterial"), HarmonyPatch(typeof(TBody), "ChangeShader"), HarmonyPatch(typeof(TBody), "SetMaterialProperty"), HarmonyPatch(typeof(TBody), "ChangeCol"), HarmonyPatch(typeof(TBody), "ChangeTex")]
		[HarmonyPostfix]
		private static void NotifyOfChange(ref TBody __instance)
		{
#if (DEBUG)
			Debug.Log("Picked up a material change");
#endif

			__instance
			.goSlot
			.Select(s => s)
			.Where(s => s != null)
			.Select(s => s.obj)
			.Where(o => o != null)
			.ToList()
			.ForEach(obj => @this.StartCoroutine(ModifySingle(obj)));		
		}
		//The following functions all deal with Props loaded by the vanilla game.
		[HarmonyPatch(typeof(BgMgr), "AddPrefabToBg"), HarmonyPatch(typeof(BgMgr), "AddAssetsBundleToBg"), HarmonyPatch(typeof(PhotoBGObjectData), "Instantiate")]
		[HarmonyPostfix]
		private static void NotifyOfGameObjLoad(ref GameObject __result)
		{
#if (DEBUG)
			Debug.Log($"Picked up an object game load: {__result.name}");
#endif

			AddToObjectDictionary(__result, Props);

			@this.StartCoroutine(ModifySingle(__result));
		}

		//This one captures nothing but background loads.
		[HarmonyPatch(typeof(BgMgr), "ChangeBg")]
		[HarmonyPostfix]
		private static void NotifyOfObjLoad(ref BgMgr __instance)
		{

			if (__instance.BgObject == null)
			{
				return;
			}

#if (DEBUG)
			Debug.Log($"Picked up a BG load: {__instance.BgObject.name}");
#endif

			AddToObjectDictionary(__instance.BgObject, Bg);

			@this.StartCoroutine(ModifySingle(__instance.BgObject));
		}

		//This patcher just supresses the shader swaps message box.
		[HarmonyPatch(typeof(NDebug), "MessageBox")]
		[HarmonyPrefix]
		private static bool SupressMSGBox(ref string __1)
		{
			if (__1.Contains("マテリアル入れ替えエラー。違うシェーダーに入れようとしました。"))
			{
#if (DEBUG)
				Debug.Log($"Supressing shader swap message box! A note in the console should still have been created though.");
#endif
				return false;
			}
			return true;
		}
	}
}
