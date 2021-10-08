using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;
using BepInEx;
using HarmonyLib;
using InnerNet;
using Hazel;
using PowerTools;
using UnityEngine;

namespace AmongUsCEDE.Core.Patches
{
	[HarmonyPatch(typeof(GameData))]
	[HarmonyPatch("RecomputeTaskCounts")]
	class TaskCountPatch
	{
		static bool Prefix(GameData __instance)
		{
			__instance.TotalTasks = 0;
			__instance.CompletedTasks = 0;
			for (int i = 0; i < __instance.AllPlayers.Count; i++)
			{
				GameData.PlayerInfo playerInfo = __instance.AllPlayers[i];
				if (!playerInfo.Disconnected && playerInfo.Tasks != null && playerInfo.Object && (PlayerControl.GameOptions.GhostsDoTasks || !playerInfo.IsDead) && playerInfo.GetRole().HasTasks)
				{
					for (int j = 0; j < playerInfo.Tasks.Count; j++)
					{
						__instance.TotalTasks++;
						if (playerInfo.Tasks[j].Complete)
						{
							__instance.CompletedTasks++;
						}
					}
				}
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(ShipStatus))]
	[HarmonyPatch("CheckTaskCompletion")]
	class CheckTaskCompletionPatch
	{
		static bool Prefix(ShipStatus __instance, ref bool __result) //if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
		{
			GameData.Instance.RecomputeTaskCounts();
			__result = false;
			return false;
		}
	}

	
}
