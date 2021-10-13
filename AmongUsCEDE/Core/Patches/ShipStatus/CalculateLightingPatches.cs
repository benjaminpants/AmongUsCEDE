using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE;
using AmongUsCEDE.LuaData;
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;

namespace AmongUsCEDE.Core.Patches
{
	internal class CalculateLightingPatches
	{


		class CalculateLightingLua
		{
			public static float Calculate(ShipStatus __instance, GameData.PlayerInfo player)
			{
				float minradius = __instance.MinLightRadius;
				float maxradius = __instance.MaxLightRadius;
				if (player == null || player.IsDead)
				{
					return maxradius;
				}
				SwitchSystem switchSystem = __instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
				float t = (float)switchSystem.Value / 255f;
				return (float)ScriptManager.RunCurrentGMFunction("CalculateLightRadius", false, (PlayerInfoLua)player, minradius, maxradius, t).Number;
			}
		}


		[HarmonyPatch(typeof(ShipStatus))]
		[HarmonyPatch("CalculateLightRadius")]
		class BasicCalculateLighting
		{
			private static bool Prefix(ShipStatus __instance, GameData.PlayerInfo player, ref float __result)
			{
				__result = CalculateLightingLua.Calculate(__instance,player);
				return false;
			}
		}

		[HarmonyPatch(typeof(AirshipStatus))]
		[HarmonyPatch("CalculateLightRadius")]
		class AirshipCalculateLighting
		{
			private static bool Prefix(AirshipStatus __instance, GameData.PlayerInfo player, ref float __result)
			{
				float num = CalculateLightingLua.Calculate(__instance, player);
				if (!player.GetRole().ImmuneToAffectors)
				{
					foreach (LightAffector lightAffector in __instance.LightAffectors)
					{
						if (player.Object && player.Object.Collider.IsTouching(lightAffector.Hitbox))
						{
							num *= lightAffector.Multiplier;
						}
					}
				}
				__result = num;
				return false;
			}
		}
	}
}
