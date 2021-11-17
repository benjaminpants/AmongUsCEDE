// AmongUsCEDE.UI.Patches.IntroBeginPatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(PlayerControl))]
[HarmonyPatch("BeginCrewmate")]
class SetRole
{
	private static void Postfix(IntroCutscene __instance)
	{
		Role role = PlayerControl.LocalPlayer.Data.GetRole();
		__instance.BackgroundBar.material.SetColor("_Color", role.RoleColor);
		__instance.TeamTitle.color = role.RoleColor;
		__instance.TeamTitle.text = role.Name;
		__instance.ImpostorText.text = role.Reveal_Text;
	}
}

