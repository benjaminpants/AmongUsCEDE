using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Extensions;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using HarmonyLib;

namespace AmongUsCEDE.Core.Patches
{

	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("CheckMurder")]
	class CheckMurderPatch
	{
		static bool Prefix(PlayerControl __instance, PlayerControl target)
		{
			if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
			{
				return false;
			}

			GameData.PlayerInfo play = __instance.Data;
			GameData.PlayerInfo playyes = target.Data;
			ScriptManager.RunCurrentGMFunction("OnUsePrimary", false, (PlayerInfoLua)play, (PlayerInfoLua)playyes);

			return false;
		}
	}
}
