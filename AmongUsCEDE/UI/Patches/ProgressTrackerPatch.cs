// AmongUsCEDE.UI.Patches.ProgressTrackerPatch
using System.Linq;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.HelperExtensions;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(ProgressTracker))]
[HarmonyPatch("FixedUpdate")]
internal class ProgressTrackerPatch
{
	private static bool Prefix(ProgressTracker __instance)
	{
		if (PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			__instance.TileParent.enabled = false;
			return false;
		}
		if (!__instance.TileParent.enabled)
		{
			__instance.TileParent.enabled = true;
		}
		GameData instance = GameData.Instance;
		if (!instance || instance.TotalTasks <= 0)
		{
			return false;
		}
		int players_that_have_no_tasks = instance.AllPlayers.ToProperList().Count((GameData.PlayerInfo p) => !p.GetRole().HasTasks);
		int num = (DestroyableSingleton<TutorialManager>.InstanceExists ? 1 : (instance.AllPlayers.Count - players_that_have_no_tasks));
		num -= instance.AllPlayers.ToProperList().Count((GameData.PlayerInfo p) => p.Disconnected);
		switch (PlayerControl.GameOptions.TaskBarMode)
		{
		case TaskBarMode.Normal:
		{
			float b = (float)instance.CompletedTasks / (float)instance.TotalTasks * (float)num;
			__instance.curValue = Mathf.Lerp(__instance.curValue, b, Time.fixedDeltaTime * 2f);
			break;
		}
		case TaskBarMode.MeetingOnly:
			if (!MeetingHud.Instance)
			{
				break;
			}
			goto case TaskBarMode.Normal;
		case TaskBarMode.Invisible:
			__instance.gameObject.SetActive(value: false);
			break;
		}
		__instance.TileParent.material.SetFloat("_Buckets", num);
		__instance.TileParent.material.SetFloat("_FullBuckets", __instance.curValue);
		return false;
	}
}
