using COM3d2.DeRim;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3d2.DeRim.Plugin
{
	class BackgroundsSupport
	{
		private static GameObject Background;
		private static int OpenCoRoutes;

		public static IEnumerator BgRun()
		{

			++OpenCoRoutes;

			yield return new WaitForSecondsRealtime(1);

			if (OpenCoRoutes-- > 1)
			{
				yield break;
			}

#if (DEBUG)
			Debug.Log($"Collecting materials and pushing them to the material editor");
#endif

			var renders = Background
			.transform
			.GetComponentsInChildren<Transform>(true)
			.Select(t => t)
			.Where(ts => ts != null)
			.Select(t => t.GetComponent<Renderer>())
			.Where(r => r != null)
			.ToList();

#if (DEBUG)
			Debug.Log($"Collected Renderers");
#endif

			renders.ForEach(r => Init.instance.StartCoroutine(ModShadows.Run(r, Init.Bg)));

#if (DEBUG)
			Debug.Log($"Manipulated Shadows");
#endif

			var materials =
			renders
			   .Where(r => r.materials != null)
			   .SelectMany(r => r.materials)
			   .ToList();

#if (DEBUG)
			Debug.Log($"Gathered this many materials: {materials.Count}");
#endif

			materials.ForEach(m => Init.instance.StartCoroutine(ModMat.Run(m, Init.Bg)));

#if (DEBUG)
			Debug.Log("Done!");
#endif

		}
	}
}
