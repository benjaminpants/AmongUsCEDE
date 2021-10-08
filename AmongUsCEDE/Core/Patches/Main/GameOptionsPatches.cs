using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.Core.CustomSettings;
using BepInEx;
using HarmonyLib;
using System.IO;
using InnerNet;
using Hazel;
using PowerTools;
using UnityEngine;
using System.Linq;

namespace AmongUsCEDE.Core
{

	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("RpcSyncSettings")]
	class HudStringPatch
	{
		static bool Prefix(PlayerControl __instance, GameOptionsData gameOptions)
		{
			GameOptionsExtension.WriteGameOptionsRPC(__instance, gameOptions);
			return false;
		}
	}


	internal class GameOptionsPatches
	{


		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("ToHudString")]
		class HudStringPatch
		{
			static void Postfix(ref string __result)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Gamemode: " + ScriptManager.CurrentGamemode.DisplayName);
				for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
				{
					builder.AppendLine(ScriptManager.CurrentGamemode.Settings[i].ToString());
				}
				__result += builder.ToString();
			}
		}





	}

}
