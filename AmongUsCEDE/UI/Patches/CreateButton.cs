// AmongUsCEDE.UI.Patches.CreateButton
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(MeetingHud))]
[HarmonyPatch("CreateButton")]
internal class CreateButton
{
	private static void Postfix(MeetingHud __instance, GameData.PlayerInfo playerInfo, ref PlayerVoteArea __result)
	{
		if (playerInfo.GetRole().CanBeSeen(PlayerControl.LocalPlayer.Data))
		{
			__result.NameText.color = playerInfo.GetRole().RoleTextColor;
		}
	}
}
