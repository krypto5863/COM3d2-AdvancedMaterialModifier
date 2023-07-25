using HarmonyLib;
using System.Linq;
using UnityEngine;
using static AdvancedMaterialModifier.SorterSenderSaver;

namespace AdvancedMaterialModifier
{
	internal static class VanillaHarmonyPatch
	{
		//This one captures the loading of maids and maid items.
		[HarmonyPatch(typeof(ImportCM), "LoadSkinMesh_R")]
		[HarmonyPostfix]
		private static void NotifyOfLoad(ref string __2, ref GameObject __result)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug("Picked up a mesh change");
#endif

			if (__result == null)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug("Gameobject was null!");
#endif
				return;
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug("Result was not null...");
#endif

			if (__2 == null)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug("Slot name was null! Aborting....");
#endif
				return;
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug("Slot name was not null");
#endif

			int? slotId = (int?)TBody.hashSlotName[__2];

			if (slotId == null)
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug("SlotID was found null so object will be treated as a game prop.");
#endif
				AddToObjectDictionary(__result, "prop");
			}
			else
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug("Adding to dictionary.");
#endif
				AddToObjectDictionary(__result, slotId.Value);
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug("Start coroutine...");
#endif

			ModifySingle(__result, true);
		}

		//This function is concerned with tracking material edits and alerting AdvancedMaterialModifier so it can correct it.
		[HarmonyPatch(typeof(TBody), "ChangeMaterial"), HarmonyPatch(typeof(TBody), "ChangeShader"), HarmonyPatch(typeof(TBody), "SetMaterialProperty"), HarmonyPatch(typeof(TBody), "ChangeCol"), HarmonyPatch(typeof(TBody), "ChangeTex")]
		[HarmonyPostfix]
		private static void NotifyOfChange(ref TBody __instance)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug("Picked up a material change");
#endif

			var objectsToMod = __instance
			.goSlot
			.Select(s => s?.obj)
			.Where(s => s != null);

			foreach (var thing in objectsToMod)
			{
				ModifySingle(thing);
			}
		}

		//Gets instances and works on them.
		[HarmonyPatch(typeof(Renderer), "materials", MethodType.Getter)]
		[HarmonyPatch(typeof(Renderer), "material", MethodType.Getter)]
		[HarmonyPatch(typeof(Renderer), "materials", MethodType.Setter)]
		[HarmonyPatch(typeof(Renderer), "material", MethodType.Setter)]
		[HarmonyPostfix]
		private static void CaptureInstancesGetter_Postfix(ref Renderer __instance)
		{
			var tBodySkin = __instance.GetParentTBodySkin();

			if (tBodySkin != null && tBodySkin.obj != null)
			{
				ModifySingle(tBodySkin.obj);
			}
		}

		//The following functions all deal with Props loaded by the vanilla game.
		[HarmonyPatch(typeof(BgMgr), "AddPrefabToBg"), HarmonyPatch(typeof(BgMgr), "AddAssetsBundleToBg"), HarmonyPatch(typeof(PhotoBGObjectData), "Instantiate")]
		[HarmonyPostfix]
		private static void NotifyOfGameObjLoad(ref GameObject __result)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Picked up an object game load: {__result.name}");
#endif
			if (__result is null)
			{
				return;
			}

			AddToObjectDictionary(__result, "prop");

			ModifySingle(__result);
		}

		//This one captures nothing but background loads.
		[HarmonyPatch(typeof(BgMgr), "ChangeBg")]
		[HarmonyPostfix]
		private static void NotifyOfObjLoad(ref BgMgr __instance)
		{
			if (__instance.BgObject is null)
			{
				return;
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Picked up a BG load: {__instance.BgObject.name}");
#endif

			AddToObjectDictionary(__instance.BgObject, "bg");

			ModifySingle(__instance.BgObject);
		}

		//This patcher just supresses the shader swaps message box.
		[HarmonyPatch(typeof(NDebug), "MessageBox")]
		[HarmonyPrefix]
		private static bool SuppressMsgBox(ref string __1)
		{
			if (__1.Contains("マテリアル入れ替えエラー。違うシェーダーに入れようとしました。"))
			{
#if (DEBUG)
				AdvancedMaterialModifier.Logger.LogDebug($"Supressing shader swap message box! A note in the console should still have been created though.");
#endif
				return false;
			}
			return true;
		}
	}
}