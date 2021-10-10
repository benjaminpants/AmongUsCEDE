using System;
using System.Linq;
using AmongUsCEDE.HelperExtensions;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace AmongUsCEDE
{
	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("SetPlayerMaterialColors", typeof(int), typeof(Renderer))]
	class ColorPatch
	{
		public static bool Prefix(int colorId, Renderer rend)
		{
			if (CustomPalette.PlayerColors.Count < Palette.PlayerColors.Length) // TODO: Remove this later or figure out a better system.
			{
				return true;
			}
			if (!rend || colorId < 0 || colorId >= CustomPalette.PlayerColors.Count)
			{
				return false;
			}
			rend.material.SetColor("_BackColor", CustomPalette.PlayerColors[colorId].Shadow);
			rend.material.SetColor("_BodyColor", CustomPalette.PlayerColors[colorId].Base);
			rend.material.SetColor("_VisorColor", CustomPalette.PlayerColors[colorId].IsSpecial ? CustomPalette.PlayerColors[colorId].Visor : Palette.VisorColor);
			return false;
		}
	}

	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("CheckColor")]
	class ColorCheckPatch
	{
		public static bool Prefix(PlayerControl __instance, byte bodyColor)
		{
			List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers.ToProperList();
			int num = 0;
			while (num++ < 100 && ((int)bodyColor >= CustomPalette.PlayerColors.Count || allPlayers.Any((GameData.PlayerInfo p) => !p.Disconnected && p.PlayerId != __instance.PlayerId && p.ColorId == (int)bodyColor)))
			{
				bodyColor = (byte)((int)(bodyColor + 1) % CustomPalette.PlayerColors.Count);
			}
			__instance.RpcSetColor(bodyColor);
			return false;
		}
	}
}
