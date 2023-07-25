using HarmonyLib;
using MeidoPhotoStudio.Plugin;
using UnityEngine;
using static AdvancedMaterialModifier.SorterSenderSaver;

namespace AdvancedMaterialModifier
{
	internal static class MpsHarmonyPatch
	{
		//MPS Specific Code. Shouldn't take effect if MPS is not present.
		[HarmonyPatch(typeof(PropManager), "AttachDragPoint")]
		[HarmonyPostfix]
		private static void NotifyOfMpsLoad(ref GameObject __0)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Picked up an MPS load: {__0.name}");
#endif

			AddToObjectDictionary(__0, "mps");

			ModifySingle(__0, true);
		}
	}
}