using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMaterialModifier
{
	internal static class SorterSenderSaver
	{
		private static readonly int[] BodySlot = { 0 };
		private static readonly int[] HeadSlot = { 1 };
		private static readonly int[] HairSlot = { 3, 4, 5, 6, 18 };
		private static readonly int[] ClothesSlot = { 7, 8, 9, 10, 11, 12, 13, 14 };
		private static readonly int[] AccSlot = { 15, 16, 17, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55 };

		public static Dictionary<GameObject, string> ObjectDictionary { get; private set; } = new Dictionary<GameObject, string>();

		private static IEnumerator _modifyAllCoroute;
		private static readonly Dictionary<MaterialGroup, IEnumerator> ModifyGroupCoroutes = new Dictionary<MaterialGroup, IEnumerator>();
		private static readonly Dictionary<GameObject, IEnumerator> ModifySingleCoroutes = new Dictionary<GameObject, IEnumerator>();

		internal static bool AddToObjectDictionary(GameObject gameObj, int slotid)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Saving new object to list!");
#endif

			if (BodySlot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameObj))
			{
				ObjectDictionary.Add(gameObj, "body");
				return true;
			}

			if (HeadSlot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameObj))
			{
				ObjectDictionary.Add(gameObj, "head");
				return true;
			}

			if (HairSlot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameObj))
			{
				ObjectDictionary.Add(gameObj, "hair");
				return true;
			}

			if (ClothesSlot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameObj))
			{
				ObjectDictionary.Add(gameObj, "clothes");
				return true;
			}

			if (AccSlot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameObj))
			{
				ObjectDictionary.Add(gameObj, "acc");
				return true;
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Returning False!");
#endif

			return false;
		}

		internal static bool AddToObjectDictionary(GameObject gameObj, string cfg)
		{
			if (gameObj == null)
			{
				return false;
			}

			ObjectDictionary[gameObj] = cfg;
			return true;
		}

		internal static void ModifyAll(bool avoidWait = false)
		{
			if (_modifyAllCoroute != null)
			{
				AdvancedMaterialModifier.This.StopCoroutine(_modifyAllCoroute);
			}

			_modifyAllCoroute = ModifyAllFunc(avoidWait);
			AdvancedMaterialModifier.This.StartCoroutine(_modifyAllCoroute);
		}

		internal static void ModifyGroup(MaterialGroup cfg, bool avoidWait = false)
		{
			if (ModifyGroupCoroutes.TryGetValue(cfg, out var coroute) && coroute != null)
			{
				AdvancedMaterialModifier.This.StopCoroutine(coroute);
			}

			ModifyGroupCoroutes[cfg] = ModifyGroupFunc(cfg, avoidWait);
			AdvancedMaterialModifier.This.StartCoroutine(ModifyGroupCoroutes[cfg]);
		}

		internal static void ModifySingle(GameObject @object, bool avoidWait = false)
		{
			if (@object is null)
			{
				return;
			}

			if (ModifySingleCoroutes.TryGetValue(@object, out var coroute) && coroute != null)
			{
				AdvancedMaterialModifier.This.StopCoroutine(coroute);
			}

			ModifySingleCoroutes[@object] = ModifySingleFunc(@object, avoidWait);
			AdvancedMaterialModifier.This.StartCoroutine(ModifySingleCoroutes[@object]);
		}

		internal static void CleanDictionary()
		{
			ObjectDictionary = ObjectDictionary
			.Select(k => k)
			.Where(k => k.Key != null && k.Value != null)
			.ToDictionary(k => k.Key, k => k.Value);
		}

		internal static IEnumerator ModifyAllFunc(bool avoidWait = false)
		{
			if (!avoidWait)
			{
				yield return new WaitForSeconds(0.10f);
			}

			CleanDictionary();

			foreach (var keyPair in ObjectDictionary)
			{
				ModifySingle(keyPair.Key, true);
			}

			_modifyAllCoroute = null;

			// ReSharper disable once RedundantJumpStatement
			yield break;
		}

		internal static IEnumerator ModifyGroupFunc(MaterialGroup cfg, bool avoidWait = false)
		{
			if (!avoidWait)
			{
				yield return new WaitForSeconds(0.10f);
			}

			CleanDictionary();

			var global = AdvancedMaterialModifier.Controls["global"] == cfg;

			foreach (var kp in ObjectDictionary)
			{
				if (global || AdvancedMaterialModifier.Controls[kp.Value] == cfg)
				{
					ModifySingle(kp.Key, true);
				}
			}

			ModifyGroupCoroutes[cfg] = null;
		}

		internal static IEnumerator ModifySingleFunc(GameObject @object, bool avoidWait = false)
		{
#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Running single task!");
#endif

			var global = AdvancedMaterialModifier.Controls["global"];

			if (!avoidWait)
			{
				yield return new WaitForSeconds(0.10f);
			}

			if (@object == null || ObjectDictionary.TryGetValue(@object, out var stringCfg) == false)
			{
				yield break;
			}

			MaterialGroup activeCfg = AdvancedMaterialModifier.Controls[stringCfg];

			if (activeCfg == null || (activeCfg.Enable == false && global.Enable == false))
			{
				yield break;
			}

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Going through renderers to send to shadow changing");
#endif

			var renderers = @object.GetAllRenderers().ToArray();

			foreach (var renderer in renderers)
			{
				AdvancedMaterialModifier.This.StartCoroutine(PropertyChanger.ChangeShadows(renderer, activeCfg));
			}

			var materials = renderers
			.SelectMany(r => r.GetAllMaterials())
			.Where(m => m != null)
			.ToList();

#if (DEBUG)
			AdvancedMaterialModifier.Logger.LogDebug($"Working... Collected this many materials: { materials.Count }");
#endif

			foreach (var material in materials)
			{
				AdvancedMaterialModifier.This.StartCoroutine(PropertyChanger.Run(material, activeCfg));
			}

			ModifySingleCoroutes[@object] = null;
		}
	}
}