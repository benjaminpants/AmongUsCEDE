// AmongUsCEDE.UI.Patches.ExilePatch
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using HarmonyLib;
using MoonSharp.Interpreter;


namespace AmongUsCEDE
{

	[HarmonyPatch(typeof(ExileController))]
	[HarmonyPatch("Begin")]
	class ExilePatch
	{
		private static void Postfix(ExileController __instance, GameData.PlayerInfo exiled)
		{
			__instance.ImpostorText.text = "";
			if (!PlayerControl.GameOptions.ConfirmImpostor)
			{
				return;
			}
			if (exiled != null)
			{
				Role role = exiled.GetRole();
				string join = "the";
				if (GameFunctions.FindAmountWithRole(role.Internal_Name) > 1)
				{
					join = exiled.PlayerName.AOrAn(false);
				}
				__instance.completeString = exiled.PlayerName + " was " + join + " " + role.Name;
				
			}

			PlayerInfoLua luafo = null;
			if (__instance.exiled != null)
			{
				luafo = (PlayerInfoLua)__instance.exiled;
			}
			DynValue man = ScriptManager.CallCurrentGMHooks("GetRemainText", luafo);
			__instance.ImpostorText.text = man == null ? "" : man.String;
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
