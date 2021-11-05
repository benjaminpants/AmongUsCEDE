using AmongUsCEDE.Core.Extensions;
using HarmonyLib;
using UnityEngine;

namespace AmongUsCEDE.Core.Patches
{
	[HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__83), nameof(PlayerControl._CoSetTasks_d__83.MoveNext))]
	class TaskTextPatch
	{
		static void Postfix(PlayerControl._CoSetTasks_d__83 __instance) //thanks to town of us for this small portion of code
		{
			if (__instance == null) return;
			Role myrole = __instance.__4__this.Data.GetRole();
			if (!myrole.HasTasks)
			{
				ImportantTextTask importantTextTask = new GameObject("_ImportantTask").AddComponent<ImportantTextTask>();
				importantTextTask.transform.SetParent(PlayerControl.LocalPlayer.transform, false);
				importantTextTask.Text = myrole.FakeTaskString + "\r\n<color=#FFFFFFFF>" + "Fake Tasks:" + "</color>"; //THIS FINALLY WORKS!!!
				__instance.__4__this.myTasks.Insert(0, importantTextTask);
			}
		}
	}
}
