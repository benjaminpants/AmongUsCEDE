// AmongUsCEDE.UI.Patches.TargetPatch
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(UseButtonManager))]
[HarmonyPatch("SetTarget")]
internal class TargetPatch
{
	private static void Postfix(UseButtonManager __instance, IUsable target)
	{
		if (target == null)
		{
			PlayerControl localPlayer = PlayerControl.LocalPlayer;
			if (((localPlayer != null) ? localPlayer.Data : null) != null && PlayerControl.LocalPlayer.Data.GetRole().CanDo(RoleSpecials.Sabotage,localPlayer.Data) && PlayerControl.LocalPlayer.CanMove)
			{
				__instance.RefreshButtons();
				__instance.currentButtonShown = __instance.otherButtons[ImageNames.SabotageButton];
				__instance.currentButtonShown.Show();
				__instance.currentButtonShown.graphic.color = UseButtonManager.EnabledColor;
				__instance.currentButtonShown.text.color = UseButtonManager.EnabledColor;
			}
		}
	}
}
