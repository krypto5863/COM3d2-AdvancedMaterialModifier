using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static COM3D2.AdvancedMaterialModifier.Plugin.FetchLibrary;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	internal class SorterSenderSaver
	{

		private static readonly int[] bodyslot = new int[] { 0 };
		private static readonly int[] headslot = new int[] { 1 };
		private static readonly int[] hairslot = new int[] { 3, 4, 5, 6, 18 };
		private static readonly int[] clothesslot = new int[] { 7, 8, 9, 10, 11, 12, 13, 14 };
		private static readonly int[] accslot = new int[] { 15, 16, 17, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55 };

		private static Dictionary<GameObject, CfgGroup> ObjectDictionary = new Dictionary<GameObject, CfgGroup>();
		public static bool AddToObjectDictionary(GameObject gameobj, int slotid)
		{

#if (DEBUG)
			Debug.Log($"Saving new object to list!");
#endif

			if (bodyslot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, Init.Body);
				return true;
			}
			else if (headslot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, Init.Head);
				return true;
			}
			else if (hairslot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, Init.Hair);
				return true;
			}
			else if (clothesslot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, Init.Clothes);
				return true;
			}
			else if (accslot.Contains(slotid) && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, Init.Acc);
				return true;
			}

#if (DEBUG)
			Debug.Log($"Returning False!");
#endif

			return false;
		}
		public static bool AddToObjectDictionary(GameObject gameobj, CfgGroup cfg)
		{
			if (gameobj != null && !ObjectDictionary.ContainsKey(gameobj))
			{
				ObjectDictionary.Add(gameobj, cfg);
				return true;
			}

			return false;
		}
		public static IEnumerator ModifyAll()
		{

			ObjectDictionary = ObjectDictionary
			.Select(k => k)
			.Where(k => k.Key != null)
			.ToDictionary(k => k.Key, k => k.Value);

			ObjectDictionary
			.Keys
			.ToList()
			.ForEach(k => Init.@this.StartCoroutine(ModifySingle(k)));

			yield return null;
		}
		public static IEnumerator ModifyAllOfGroup(CfgGroup cfg)
		{

			ObjectDictionary = ObjectDictionary
			.Select(k => k)
			.Where(k => k.Key != null)
			.ToDictionary(k => k.Key, k => k.Value);

			ObjectDictionary
			.Select(kp => kp)
			.Where(kp => kp.Value == cfg)
			.ToList()
			.ForEach(k => Init.@this.StartCoroutine(ModifySingle(k.Key)));

			yield return null;
		}
		public static IEnumerator ModifySingle(GameObject @object)
		{
#if (DEBUG)
			Debug.Log($"Running single task!");
#endif

			CfgGroup activecfg = null;

			if (@object == null || !ObjectDictionary.TryGetValue(@object, out activecfg) || activecfg.Enable.Value != true)
			{
				yield break;
			}

#if (DEBUG)
			Debug.Log($"Going through renderers to send to shadow changing");
#endif

			List<Renderer> renderers = GetAllRenderers(@object).ToList();

			renderers
			.ForEach(r => Init.@this.StartCoroutine(PropertyChanger.ChangeShadows(r, activecfg)));

			List<Material> materials = renderers
			.Select(r => GetAllMaterials(r))
			.SelectMany(m => m)
			.Where(m => m != null).ToList();

#if (DEBUG)
			Debug.Log($"Working... Collected this many materials: { materials.Count }");
#endif

			materials
			.ForEach(m => Init.@this.StartCoroutine(PropertyChanger.Run(m, activecfg)));

			yield return null;
		}
	}
}