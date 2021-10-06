// AmongUsCEDE.Core.Patches.ConsolePatch
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Console))]
[HarmonyPatch("CanUse")]
internal class ConsolePatch
{
	private static bool Prefix(Console __instance, GameData.PlayerInfo pc)
	{
		float num = Vector2.Distance(pc.Object.GetTruePosition(), __instance.transform.position);
		if (!(num <= __instance.UsableDistance))
		{
			return true;
		}
		if (!__instance.AllowImpostor)
		{
			pc.IsImpostor = !pc.GetRole().HasTasks;
		}
		return true;
	}

	private static void Postfix(Console __instance, GameData.PlayerInfo pc)
	{
		if (pc.IsImpostor)
		{
			pc.IsImpostor = false;
		}
	}
}
