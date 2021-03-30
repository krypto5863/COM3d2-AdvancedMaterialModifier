using HarmonyLib;
using System.Linq;
using System;
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

			if (__result == null) 
			{
#if (DEBUG)
				Debug.Log("Gameobject was null!");
#endif
				return;
			}

#if (DEBUG)
			Debug.Log("Result was not null...");
#endif

			if (__2 == null)
			{
#if (DEBUG)
				Debug.Log("Slot name was null! Aborting....");
#endif
				return;
			}

#if (DEBUG)
			Debug.Log("Slot name was not null");
#endif

			TBody.SlotID slotid = TBody.SlotID.end;

			try
			{
				slotid = (TBody.SlotID)Enum.Parse(typeof(TBody.SlotID), __2.ToLower());
			}
			catch 
			{  
			
			}

			if (slotid.Equals(TBody.SlotID.end))
			{
#if (DEBUG)
				Debug.Log("SlotID was found null so object will be treated as a game prop.");
#endif
				AddToObjectDictionary(__result, Props);
			}
			else
			{
#if (DEBUG)
				Debug.Log("Adding to dictionary.");
#endif
				AddToObjectDictionary(__result, slotid);
			}

#if (DEBUG)
			Debug.Log("Start coroutine...");
#endif

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
			.GetListParents()
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
