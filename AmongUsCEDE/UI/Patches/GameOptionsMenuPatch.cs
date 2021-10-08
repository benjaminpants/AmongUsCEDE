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
	[HarmonyPatch(typeof(GameOptionsMenu))]
	[HarmonyPatch("Start")]
	class GameOptionsMenuPatch
	{
		static void Prefix(GameOptionsMenu __instance)
		{
			//DebugLog.ShowMessage("I AM RUNNING" + __instance.transform.FindChild("CrewmateVision").gameObject);
			//GameObject.Destroy(__instance.transform.FindChild("CrewmateVision").gameObject);
			//GameObject.Destroy(__instance.transform.FindChild("ImpostorVision").gameObject);

			

		}
		static void Postfix(GameOptionsMenu __instance)
		{
			var numberoption = GameObject.FindObjectsOfType<NumberOption>().FirstOrDefault();

			float start_y = -8.35f;

			for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
			{
				Setting setting = ScriptManager.CurrentGamemode.Settings[i];
				if (setting.settingtype == SettingType.Float || setting.settingtype == SettingType.Int)
				{
					NumberOption opt = GameObject.Instantiate<NumberOption>(numberoption, numberoption.transform.parent);
					opt.transform.localPosition = new Vector3(opt.transform.localPosition.x, start_y - (0.5f * i), opt.transform.localPosition.z);
					opt.Title = (StringNames)i; //clever yet stupid way of checking what setting it is
					opt.TitleText.text = setting.display_name;
					if (setting.settingtype == SettingType.Int)
					{
						opt.ValidRange = new FloatRange((int)setting.Min, (int)setting.Max);
						opt.Value = (int)setting.Value;
					}
					else
					{
						opt.ValidRange = new FloatRange((float)setting.Min, (float)setting.Max);
						opt.Value = (float)setting.Value;
					}
					opt.Increment = setting.Increment;
					opt.ValueText.text = opt.Value.ToString();
					opt.oldValue = opt.Value;
					opt.ZeroIsInfinity = false;
					opt.FormatString = (setting.settingtype == SettingType.Int ? "0" : "0.0");
					opt.SuffixType = (setting.addend == "x" ? NumberSuffixes.Multiplier : setting.addend == "s" ? NumberSuffixes.Seconds : NumberSuffixes.None);
					opt.OnValueChanged = (Action<OptionBehaviour>)GameOptionsExtension.UpdateSetting;
				}
			}
			
		}

	}
}
