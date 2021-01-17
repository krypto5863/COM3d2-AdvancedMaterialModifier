using COM3D2.MeidoPhotoStudio.Plugin;
using HarmonyLib;
using UnityEngine;
using static COM3D2.AdvancedMaterialModifier.Plugin.Init;
using static COM3D2.AdvancedMaterialModifier.Plugin.SorterSenderSaver;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	internal class MPSHarmonyPatch
	{
		//MPS Specific Code. Shouldn't take effect if MPS is not present.
		[HarmonyPatch(typeof(PropManager), "AttachDragPoint")]
		[HarmonyPostfix]
		private static void NotifyOfMPSLoad(ref GameObject __0)
		{
#if (DEBUG)
			Debug.Log($"Picked up an MPS load: {__0.name}");
#endif

			AddToObjectDictionary(__0, MpsProps);

			Init.@this.StartCoroutine(ModifySingle(__0));
		}
	}
}
