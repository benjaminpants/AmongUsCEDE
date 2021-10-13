// AmongUsCEDE.Core.Patches.PlayerControlFixedUpdate
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(PlayerControl))]
[HarmonyPatch("FixedUpdate")]
internal class PlayerControlFixedUpdate
{
	private static void Postfix(PlayerControl __instance)
	{
		GameData.PlayerInfo data = __instance.Data;
		if (data != null && __instance.AmOwner && data.GetRole().CanDo(RoleSpecials.Primary, data) && __instance.CanMove && !data.IsDead)
		{
			__instance.SetKillTimer(__instance.killTimer - Time.fixedDeltaTime);
			PlayerControl target = __instance.FindClosestTarget();
			DestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(target);
		}
	}
}
