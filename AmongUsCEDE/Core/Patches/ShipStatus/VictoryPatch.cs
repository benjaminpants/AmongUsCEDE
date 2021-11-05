// AmongUsCEDE.Core.Patches.VictoryPatch
using AmongUsCEDE;
using AmongUsCEDE.Core;
using HarmonyLib;

[HarmonyPatch(typeof(ShipStatus))]
[HarmonyPatch("CheckEndCriteria")]
class VictoryPatch
{
	private static bool Prefix(ShipStatus __instance)
	{
		if (!GameData.Instance)
		{
			return false;
		}
		bool IsSabotageEnd = false;
		//there used to be a lot of copied stuff here but due to my incompetence its no longer here
		if (__instance.Systems.ContainsKey(SystemTypes.LifeSupp))
		{
			ISystemType funnySystem = __instance.Systems[SystemTypes.LifeSupp];
			LifeSuppSystemType lifeSuppSystemType = funnySystem.Cast<LifeSuppSystemType>();
			if (lifeSuppSystemType.Countdown < 0f)
			{
				IsSabotageEnd = true;
				lifeSuppSystemType.Countdown = 10000f;
			}
		}
		if (__instance.Systems.ContainsKey(SystemTypes.Laboratory))
		{
			ISystemType funnySystem = __instance.Systems[SystemTypes.Laboratory];
			ICriticalSabotage critsystem = funnySystem.Cast<ICriticalSabotage>();
			if (critsystem.Countdown < 0f)
			{
				IsSabotageEnd = true;
				critsystem.ClearSabotage();
			}
		}
		else if (__instance.Systems.ContainsKey(SystemTypes.Reactor))
		{
			ISystemType funnySystem = __instance.Systems[SystemTypes.Reactor];
			ICriticalSabotage critsystem = funnySystem.Cast<ICriticalSabotage>();
			if (critsystem.Countdown < 0f)
			{
				IsSabotageEnd = true;
				critsystem.ClearSabotage();
			}
		}

		ScriptManager.RunCurrentGMFunction("CheckEndCriteria", false, GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks, IsSabotageEnd);
		return false;
	}
}


[HarmonyPatch(typeof(ShipStatus))]
[HarmonyPatch("IsGameOverDueToDeath")]
class PreventHangsAndStuffDueToDeathGameOver
{
	private static bool Prefix(bool __result)
	{
		__result = false;
		return false;
	}
}
