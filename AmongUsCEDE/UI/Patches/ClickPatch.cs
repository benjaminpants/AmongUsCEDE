// AmongUsCEDE.UI.Patches.ClickPatch
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(UseButtonManager))]
[HarmonyPatch("DoClick")]
internal class ClickPatch
{
	private static void Prefix(UseButtonManager __instance)
	{
		if (PlayerControl.LocalPlayer.Data.GetRole().AvailableSpecials.Contains(RoleSpecials.Sabotage))
		{
			PlayerControl.LocalPlayer.Data.IsImpostor = true;
		}
	}

	private static void Postfix(UseButtonManager __instance)
	{
		if (PlayerControl.LocalPlayer.Data.IsImpostor)
		{
			PlayerControl.LocalPlayer.Data.IsImpostor = false;
		}
	}
}
