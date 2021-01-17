using COM3d2.DeRim;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriLib;
using UnityEngine;

namespace COM3d2.DeRim.Plugin
{
	class GamePropsSupport
	{

		private static List<GameObject> Objects = new List<GameObject>();

		private static int OpenCoRoutes = 0;

		public static IEnumerator GpsRun()
		{

			++OpenCoRoutes;

			yield return new WaitForSecondsRealtime(1);

			if (OpenCoRoutes-- > 1)
			{
				yield break;
			}

#if (DEBUG)
			Debug.Log("Attempting to remove null objects!");
#endif
			Objects = Objects
				.Where(p => p != null)
				.ToList();

#if (DEBUG)
			Debug.Log($"Collecting materials and pushing them to the material editor: {Objects.Count}");
#endif

			var renders = Objects
			.Where(o => o != null)
			.Select(o => o.transform)
			.Where(t => t != null)
			.Select(t => t.GetComponentsInChildren<Transform>(true))
			.Where(ts => ts != null)
			.SelectMany(ts => ts)
			.Select(t => t.GetComponent<Renderer>())
			.Where(r => r != null)
			.ToList();

			renders.ForEach(r => Init.instance.StartCoroutine(ModShadows.Run(r, Init.Props)));

			var materials =
			renders
			   .Where(r => r.materials != null)
			   .SelectMany(r => r.materials)
			   .ToList();

#if (DEBUG)
			Debug.Log($"Gathered this many materials: {materials.Count}");
#endif

			materials.ForEach(m => Init.instance.StartCoroutine(ModMat.Run(m, Init.Props)));

#if (DEBUG)
			Debug.Log("Done!");
#endif

		}
	}
}
