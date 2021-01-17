using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COM3D2.AdvancedMaterialModifier.Plugin
{
	internal class FetchLibrary
	{
		public static IEnumerable<Material> GetAllMaterials(Maid maid, int[] slotlist)
		{
			return
				maid
				.body0
				.goSlot
				.Where(s => slotlist.Contains((int)s.SlotId))
				.Select(s => s.obj)
				.Where(o => o != null)
				.Select(o => o.transform)
				.Where(t => t != null)
				.Select(t => t.GetComponentsInChildren<Transform>(true))
				.Where(ts => ts != null)
				.SelectMany(ts => ts)
				.Select(t => t.GetComponent<Renderer>())
				.Where(r => r != null && r.material != null)
				.SelectMany(r => r.materials);
		}
		public static IEnumerable<Material> GetAllMaterials(GameObject @object)
		{
			return
			@object
			.transform
			.GetComponentsInChildren<Transform>(true)
			.Select(t => t)
			.Where(ts => ts != null)
			.Select(t => t.GetComponent<Renderer>())
			.Where(r => r != null && r.material != null)
			.SelectMany(r => r.materials);
		}
		public static IEnumerable<Material> GetAllMaterials(Renderer Render)
		{
			return
				Render
				.materials
				.Where(m => m != null);
		}
		public static IEnumerable<Renderer> GetAllRenderers(Maid maid, int[] slotlist)
		{
			return
				maid
				.body0
				.goSlot
				.Where(s => slotlist.Contains((int)s.SlotId))
				.Select(s => s.obj)
				.Where(o => o != null)
				.Select(o => o.transform)
				.Where(t => t != null)
				.Select(t => t.GetComponentsInChildren<Transform>(true))
				.Where(ts => ts != null)
				.SelectMany(ts => ts)
				.Select(t => t.GetComponent<Renderer>())
				.Where(r => r != null);
		}
		public static IEnumerable<Renderer> GetAllRenderers(GameObject @object)
		{
			return
				@object
				.transform
				.GetComponentsInChildren<Transform>(true)
				.Select(t => t)
				.Where(ts => ts != null)
				.Select(t => t.GetComponent<Renderer>())
				.Where(r => r != null);
		}
	}
}
