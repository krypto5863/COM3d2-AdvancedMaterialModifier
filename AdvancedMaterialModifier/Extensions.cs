using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMaterialModifier
{
	internal static class Extensions
	{
		internal static IEnumerable<Material> GetAllMaterials(this Maid maid, int[] slotList)
		{
			return
				maid
				.body0
				.goSlot
				.Where(s => slotList.Contains((int)s.SlotId))
				.SelectMany(s => s.obj?.GetAllMaterials());
		}

		internal static IEnumerable<Material> GetAllMaterials(this GameObject @object)
		{
			return
			@object
			.GetAllRenderers()
			.SelectMany(r => r?.GetAllMaterials());
		}

		internal static IEnumerable<Material> GetAllMaterials(this Renderer render)
		{
			return
				render
				.sharedMaterials
				.Where(m => m != null);
		}

		internal static IEnumerable<Renderer> GetAllRenderers(this Maid maid, int[] slotList)
		{
			return
				maid
				.body0
				.goSlot
				.Where(s => slotList.Contains((int)s.SlotId))
				.SelectMany(s => s.obj?.GetAllRenderers());
		}

		internal static IEnumerable<Renderer> GetAllRenderers(this GameObject @object)
		{
			return
				@object
				.transform
				.GetComponentsInChildren<Transform>(true)
				.Select(t => t?.GetComponent<Renderer>())
				.Where(r => r != null);
		}
		public static TBodySkin GetParentTBodySkin(this Renderer render)
		{
			return render?.GetComponentInParent<TBody>()?
				.goSlot?
				.FirstOrDefault(r => r?.obj?.transform.GetComponentInChildren<Renderer>() == render);
		}
	}
}