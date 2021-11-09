using AmongUsCEDE.Core.Extensions;
using HarmonyLib;
using UnityEngine;
using AmongUsCEDE.Core;

namespace AmongUsCEDE.Core.Patches
{
	[HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__83), nameof(PlayerControl._CoSetTasks_d__83.MoveNext))]
	public class TaskTextPatch
	{
		public static void Postfix(PlayerControl._CoSetTasks_d__83 __instance) //thanks to town of us for this small portion of code
		{
			if (__instance == null) return;
			__instance.__4__this.SetImportantText();
		}
	}
}
