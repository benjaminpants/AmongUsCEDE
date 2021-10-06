// AmongUsCEDE.UI.Patches.ExilePatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.Extensions;
using HarmonyLib;

[HarmonyPatch(typeof(ExileController))]
[HarmonyPatch("Begin")]
internal class ExilePatch
{
	private static void Postfix(ExileController __instance, GameData.PlayerInfo exiled)
	{
		if (!PlayerControl.GameOptions.ConfirmImpostor)
		{
			__instance.ImpostorText.text = "";
		}
		else if (exiled != null)
		{
			Role role = exiled.GetRole();
			string join = "the";
			if (GameFunctions.FindAmountWithRole(role.UUID) > 1)
			{
				join = exiled.PlayerName.AOrAn(false);
			}
			__instance.completeString = exiled.PlayerName + " was " + join + " " + role.RoleName;
			__instance.ImpostorText.text = "";
		}
	}
}
