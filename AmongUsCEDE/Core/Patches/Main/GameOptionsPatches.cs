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
	internal class GameOptionsPatches
	{
		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("Deserialize")]
		class DeserializePatch
		{

		}
	}
}
