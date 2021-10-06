// AmongUsCEDE.Core.Patches.FindClosestTargetUpdate
using AmongUsCEDE;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using HarmonyLib;
using MoonSharp.Interpreter;

[HarmonyPatch(typeof(PlayerControl))]
[HarmonyPatch("FindClosestTarget")]
internal class FindClosestTargetUpdate
{
	private static void Postfix(PlayerControl __instance, ref PlayerControl __result)
	{
		if (__instance.Data.GetRole().PrimaryTargets == PrimaryTarget.Self)
		{
			__result = __instance;
			return;
		}
		if (__result == null)
		{
			if (__instance.Data.GetRole().PrimaryTargets == PrimaryTarget.SelfIfNone)
			{
				DynValue val3 = ScriptManager.RunCurrentGMFunction("CanUsePrimary", false, (PlayerInfoLua)__instance, (PlayerInfoLua)__instance);
				if (val3.Type == DataType.Boolean && val3.Boolean)
				{
					__result = __instance;
				}
			}
			return;
		}
		DynValue val = ScriptManager.RunCurrentGMFunction("CanUsePrimary", false, (PlayerInfoLua)__instance, (PlayerInfoLua)__result);
		if (val.Type != DataType.Boolean || val.Boolean)
		{
			return;
		}
		__result = null;
		if (__instance.Data.GetRole().PrimaryTargets == PrimaryTarget.SelfIfNone)
		{
			DynValue val2 = ScriptManager.RunCurrentGMFunction("CanUsePrimary", false, (PlayerInfoLua)__instance, (PlayerInfoLua)__instance);
			if (val2.Type == DataType.Boolean && val2.Boolean)
			{
				__result = __instance;
			}
		}
	}
}
