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

	private static void Postfix(Console __instance, GameData.PlayerInfo pc, ref bool couldUse)
	{
		PlayerControl @object = pc.Object;
		Vector2 truePosition = @object.GetTruePosition();
		couldUse = ((!pc.IsDead || (PlayerControl.GameOptions.GhostsDoTasks && !__instance.GhostsIgnored)) && @object.CanMove && (__instance.AllowImpostor || pc.GetRole().HasTasks) && (!__instance.onlySameRoom || __instance.InRoom(truePosition)) && (!__instance.onlyFromBelow || truePosition.y < __instance.transform.position.y) && __instance.FindTask(@object));
		if (pc.IsImpostor)
		{
			pc.IsImpostor = false;
		}
	}
}
