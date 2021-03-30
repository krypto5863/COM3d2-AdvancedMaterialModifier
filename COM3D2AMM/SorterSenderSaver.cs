using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static COM3D2.AdvancedMaterialModifier.Plugin.FetchLibrary;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	internal class SorterSenderSaver
	{

		private static readonly TBody.SlotID[] bodyslot = new TBody.SlotID[]
		{
			TBody.SlotID.body,
			TBody.SlotID.chikubi,
			TBody.SlotID.chinko,
			TBody.SlotID.asshair,
			TBody.SlotID.underhair
		};

		private static readonly TBody.SlotID[] headslot = new TBody.SlotID[]
		{
			TBody.SlotID.head,
			TBody.SlotID.kousoku_lower,
			TBody.SlotID.kousoku_upper
		};

		private static readonly TBody.SlotID[] hairslot = new TBody.SlotID[]
		{
			TBody.SlotID.hairAho,
			TBody.SlotID.hairF,
			TBody.SlotID.hairR,
			TBody.SlotID.hairS,
			TBody.SlotID.hairS_2,
			TBody.SlotID.hairT,
			TBody.SlotID.hairT_2
		};

		private static readonly TBody.SlotID[] clothesslot = new TBody.SlotID[]
		{
			TBody.SlotID.bra,
			TBody.SlotID.glove,
			TBody.SlotID.headset,
			TBody.SlotID.mizugi,
			TBody.SlotID.mizugi_buttom,
			TBody.SlotID.mizugi_top,
			TBody.SlotID.onepiece,
			TBody.SlotID.panz,
			TBody.SlotID.shoes,
			TBody.SlotID.skirt,
			TBody.SlotID.stkg,
			TBody.SlotID.wear
		};

		private static readonly TBody.SlotID[] accslot = new TBody.SlotID[]
		{
			TBody.SlotID.accAnl,
			TBody.SlotID.accAshi,
			TBody.SlotID.accAshi_2,
			TBody.SlotID.accFace,
			TBody.SlotID.accHa,
			TBody.SlotID.accHana,
			TBody.SlotID.accHat,
			TBody.SlotID.accHead,
			TBody.SlotID.accHead_2,
			TBody.SlotID.accHeso,
			TBody.SlotID.accKamiSubL,
			TBody.SlotID.accKamiSubR,
			TBody.SlotID.accKami_1_,
			TBody.SlotID.accKami_2_,
			TBody.SlotID.accKami_3_,
			TBody.SlotID.accKoshi,
			TBody.SlotID.accKubi,
			TBody.SlotID.accKubiwa,
			TBody.SlotID.accMiMiL,
			TBody.SlotID.accMiMiR,
			TBody.SlotID.accNipL,
			TBody.SlotID.accNipR,
			TBody.SlotID.accSenaka,
			TBody.SlotID.accShippo,
			TBody.SlotID.accUde,
			TBody.SlotID.accUde_2,
			TBody.SlotID.accVag,
			TBody.SlotID.accXXX,
			TBody.SlotID.HandItemL,
			TBody.SlotID.HandItemR,
			TBody.SlotID.kubiwa,
			TBody.SlotID.megane
		};

		private static Dictionary<GameObject, CfgGroup> ObjectDictionary = new Dictionary<GameObject, CfgGroup>();
		public static bool AddToObjectDictionary(GameObject gameobj, TBody.SlotID slotid)
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