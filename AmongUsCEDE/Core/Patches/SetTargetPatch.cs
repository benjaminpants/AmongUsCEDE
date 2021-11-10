using AmongUsCEDE.Core;
using HarmonyLib;
using AmongUsCEDE.Extensions;
using AmongUsCEDE.Core.Extensions;

[HarmonyPatch(typeof(KillButton))]
[HarmonyPatch("SetTarget")]
internal class SetTargetPatch
{
	private static bool Prefix(KillButton __instance, PlayerControl target)
	{
		if (!PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data == null || !PlayerControl.LocalPlayer.Data.Role)
		{
			return false;
		}
		if (__instance.currentTarget && __instance.currentTarget != target)
		{
			__instance.currentTarget.ToggleHighlight(false, RoleTeamTypes.Crewmate);
		}
		__instance.currentTarget = target;
		if (__instance.currentTarget)
		{
			__instance.currentTarget.ToggleHighlight(true, RoleTeamTypes.Crewmate);
			__instance.SetEnabled();
			return false;
		}
		__instance.SetDisabled();
		return false;
	}
}

[HarmonyPatch(typeof(PlayerControl))]
[HarmonyPatch("ToggleHighlight")]
internal class TargetColors
{
	static bool Prefix(PlayerControl __instance, bool active)
	{
		if (active)
		{
			__instance.myRend.material.SetFloat("_Outline", 1f);
			__instance.myRend.material.SetColor("_OutlineColor", PlayerControl.LocalPlayer.Data.GetRole().RoleColor);
			return false;
		}
		__instance.myRend.material.SetFloat("_Outline", 0f);
		return false;
	}
}
