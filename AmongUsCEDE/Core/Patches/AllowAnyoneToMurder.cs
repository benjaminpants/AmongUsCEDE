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
	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("MurderPlayer")]
	class AllowAnyoneToMurder
	{
		static void Prefix(PlayerControl __instance, PlayerControl target) 
		{
			__instance.Data.IsImpostor = true;
			target.Data.IsImpostor = false;
		}

		static void Postfix(PlayerControl __instance)
		{
			__instance.Data.IsImpostor = false;


		}
	}
}
