// AmongUsCEDE.UI.Patches.IntroBeginPatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(IntroCutscene))]
[HarmonyPatch("BeginCrewmate")]
class IntroBeginPatch
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

[HarmonyPatch(typeof(IntroCutscene))]
[HarmonyPatch("SetUpRoleText")]
class SetUpRoleTextPatch
{
	private static void Postfix(IntroCutscene __instance)
	{
		__instance.RoleText.text = "No Modifier";
		__instance.RoleBlurbText.text = "This screen is really stupid";
		__instance.RoleText.color = Palette.White;
		__instance.YouAreText.color = Palette.White;
		__instance.RoleBlurbText.color = Palette.White;
	}
}
