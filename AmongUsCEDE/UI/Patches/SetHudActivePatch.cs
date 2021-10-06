// AmongUsCEDE.UI.Patches.SetHudActivePatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(HudManager))]
[HarmonyPatch("SetHudActive")]
internal class SetHudActivePatch
{
	private static bool Prefix(HudManager __instance, bool isActive)
	{
		__instance.UseButton.gameObject.SetActive(isActive);
		__instance.UseButton.Refresh();
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		GameData.PlayerInfo playerInfo = ((localPlayer != null) ? localPlayer.Data : null);
		Role role = playerInfo.GetRole();
		__instance.ReportButton.gameObject.SetActive(isActive && role.AvailableSpecials.Contains(RoleSpecials.Report));
		__instance.KillButton.gameObject.SetActive(isActive && role.AvailableSpecials.Contains(RoleSpecials.Primary) && !playerInfo.IsDead);
		__instance.TaskText.transform.parent.gameObject.SetActive(isActive);
		__instance.roomTracker.gameObject.SetActive(isActive);
		return false;
	}
}
