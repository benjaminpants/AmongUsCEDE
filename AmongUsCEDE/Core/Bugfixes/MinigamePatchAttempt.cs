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
	[HarmonyPatch(typeof(Minigame))]
	[HarmonyPatch("Close",new Type[0])]
	class MinigamePatchAttempt
	{
		static bool Prefix(Minigame __instance) 
		{
			DebugLog.ShowMessage("Uhhhh");
			if (__instance.amClosing != Minigame.CloseState.Closing)
			{
				if (__instance.CloseSound && Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(__instance.CloseSound, false, 1f);
				}
				ConsoleJoystick.SetMode_Menu();
				if (PlayerControl.LocalPlayer)
				{
					PlayerControl.HideCursorTemporarily();
				}
				__instance.amClosing = Minigame.CloseState.Closing;
				__instance.StartCoroutine(__instance.CoDestroySelf());
				return false;
			}
			UnityEngine.Object.Destroy(__instance.gameObject);
			return false;
		}

	}
}
