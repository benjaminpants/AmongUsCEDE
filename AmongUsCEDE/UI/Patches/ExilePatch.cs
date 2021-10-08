// AmongUsCEDE.UI.Patches.ExilePatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using HarmonyLib;


namespace AmongUsCEDE
{

	[HarmonyPatch(typeof(ExileController))]
	[HarmonyPatch("Begin")]
	class ExilePatch
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
				if (GameFunctions.FindAmountWithRole(role.Internal_Name) > 1)
				{
					join = exiled.PlayerName.AOrAn(false);
				}
				__instance.completeString = exiled.PlayerName + " was " + join + " " + role.Name;
				__instance.ImpostorText.text = "";
			}
		}
	}

	[HarmonyPatch(typeof(ExileController))]
	[HarmonyPatch("WrapUp")]
	class ExileFinalizePatch
	{
		static void Postfix(ExileController __instance)
		{
			PlayerInfoLua luafo = null;
			if (__instance.exiled != null)
			{
				luafo = (PlayerInfoLua)__instance.exiled;
			}
			ScriptManager.CallCurrentGMHooks("OnEject",luafo);
		}
	}

}
