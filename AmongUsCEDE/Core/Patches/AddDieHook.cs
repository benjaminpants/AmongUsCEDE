using AmongUsCEDE;
using AmongUsCEDE.Core;
using AmongUsCEDE.LuaData;
using HarmonyLib;

namespace AmongUsCEDE.Core.Patches
{
	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("Die")]
	class AddDieHook
	{
		static void Postfix(PlayerControl __instance, DeathReason reason)
		{
			ScriptManager.CallCurrentGMHooks("OnPlayerDeath", (PlayerInfoLua)__instance.Data, reason);
		}
	}
}
