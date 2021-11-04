using System;
using System.Collections.Generic;
using System.Linq;
using AmongUsCEDE.Core;
using AmongUsCEDE.HelperExtensions;
using Assets.CoreScripts;
using AmongUsCEDE.Core.CustomSettings;
using HarmonyLib;
using UnityEngine;

namespace AmongUsCEDE.UI.Patches
{
	internal class GameOptionsButtonPatches
	{
		[HarmonyPatch(typeof(NumberOption))]
		[HarmonyPatch("OnEnable")]
		class NumberOptionPatch
		{
			static bool Prefix(NumberOption __instance)
			{
				if ((int)__instance.Title >= AmongUsCEDE.HardcodedSettingStringOverrideStart)
				{
					__instance.FixedUpdate();
					return false;
				}
				if ((int)__instance.Title <= AmongUsCEDE.MaxSettingAmount)
				{
					__instance.FixedUpdate();
					return false;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(ToggleOption))]
		[HarmonyPatch("OnEnable")]
		class ToggleOptionPatch
		{
			static bool Prefix(ToggleOption __instance)
			{
				if ((int)__instance.Title >= AmongUsCEDE.HardcodedSettingStringOverrideStart)
				{
					__instance.FixedUpdate();
					return false;
				}
				if ((int)__instance.Title <= AmongUsCEDE.MaxSettingAmount)
				{
					__instance.FixedUpdate();
					return false;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(StringOption))]
		[HarmonyPatch("OnEnable")]
		class StringOptionPatch
		{
			static bool Prefix(StringOption __instance)
			{
				if ((int)__instance.Title >= AmongUsCEDE.HardcodedSettingStringOverrideStart)
				{
					__instance.FixedUpdate();
					return false;
				}
				if ((int)__instance.Title <= AmongUsCEDE.MaxSettingAmount)
				{
					__instance.FixedUpdate();
					return false;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(StringOption))]
		[HarmonyPatch("FixedUpdate")]
		class StringOptionChangePatch
		{
			static bool Prefix(StringOption __instance)
			{
				if (__instance == null) return false;
				if ((int)__instance.Title <= AmongUsCEDE.MaxSettingAmount)
				{
					if (__instance.oldValue != __instance.Value)
					{
						__instance.oldValue = __instance.Value;
						__instance.ValueText.text = (ScriptManager.CurrentGamemode.Settings[(int)__instance.Title] as StringListSetting).Strings[__instance.Value];
					}
					return false;
				}
				if ((int)__instance.Title >= AmongUsCEDE.HardcodedSettingStringOverrideStart)
				{
					if (__instance.oldValue != __instance.Value)
					{
						__instance.oldValue = __instance.Value;
						__instance.ValueText.text = (CEManager.HardcodedSettings[(int)__instance.Title - AmongUsCEDE.HardcodedSettingStringOverrideStart] as StringListSetting).Strings[__instance.Value];
					}
					return false;
				}
				return true;
			}
		}
	}
}
