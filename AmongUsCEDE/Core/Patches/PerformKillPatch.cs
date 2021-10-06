// AmongUsCEDE.Core.Patches.PerformKillPatch
using AmongUsCEDE.Core;
using HarmonyLib;

[HarmonyPatch(typeof(KillButtonManager))]
[HarmonyPatch("PerformKill")]
internal class PerformKillPatch
{
	private static bool Prefix(KillButtonManager __instance)
	{
		if (__instance.isActiveAndEnabled && (bool)__instance.CurrentTarget && !__instance.isCoolingDown && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove)
		{
			GameFunctions.SendKillRequestRPC(PlayerControl.LocalPlayer, __instance.CurrentTarget);
			__instance.SetTarget(null);
			PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
		}
		return false;
	}
}
