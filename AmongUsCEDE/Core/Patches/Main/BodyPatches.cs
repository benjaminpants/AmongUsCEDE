using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace AmongUsCEDE.Core.Patches
{

	[HarmonyPatch(typeof(DeadBody))]
	[HarmonyPatch("OnClick")]
	class BodyPatches
	{
		static bool Prefix()
		{
			//TODO: finish this
			return true;
		}
	}
}
