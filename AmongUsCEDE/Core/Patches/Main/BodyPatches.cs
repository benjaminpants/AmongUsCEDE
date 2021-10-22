using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Extensions;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

namespace AmongUsCEDE.Core.Patches
{

	[HarmonyPatch(typeof(DeadBody))]
	[HarmonyPatch("OnClick")]
	class BodyPatches
	{
		static bool Prefix()
		{
			return PlayerControl.LocalPlayer.Data.GetRole().CanDo(RoleSpecials.Report,PlayerControl.LocalPlayer.Data);
		}
	}
}
