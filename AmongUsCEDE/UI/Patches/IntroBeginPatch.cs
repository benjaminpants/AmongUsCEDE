// AmongUsCEDE.UI.Patches.IntroBeginPatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(IntroCutscene))]
[HarmonyPatch("BeginCrewmate")]
internal class IntroBeginPatch
{
	private static void Postfix(IntroCutscene __instance)
	{
		Role role = PlayerControl.LocalPlayer.Data.GetRole();
		__instance.BackgroundBar.material.SetColor("_Color", role.RoleColor);
		__instance.Title.color = role.RoleColor;
		__instance.Title.text = role.Name;
		__instance.ImpostorText.text = role.Reveal_Text;
	}
}
