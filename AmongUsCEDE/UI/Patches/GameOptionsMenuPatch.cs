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

	internal static class GameOptionsMenuPatches
	{

		private static Scroller scroller;
		[HarmonyPatch(typeof(GameOptionsMenu))]
		[HarmonyPatch("Start")]
		class GameOptionsMenuPatch
		{
			static bool Prefix(GameOptionsMenu __instance)
			{
				var numberoption = GameObject.FindObjectsOfType<NumberOption>().FirstOrDefault();

				var toggleoption = GameObject.FindObjectsOfType<ToggleOption>().FirstOrDefault();

				var stringoption = GameObject.FindObjectsOfType<StringOption>().FirstOrDefault();

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
					else if (setting.settingtype == SettingType.Toggle)
					{
						ToggleOption opt = GameObject.Instantiate<ToggleOption>(toggleoption, toggleoption.transform.parent);
						opt.transform.localPosition = new Vector3(opt.transform.localPosition.x, start_y - (0.5f * i), opt.transform.localPosition.z);
						opt.Title = (StringNames)i; //clever yet stupid way of checking what setting it is
						opt.TitleText.text = setting.display_name;
						opt.CheckMark.enabled = (bool)setting.Value;
						opt.OnValueChanged = (Action<OptionBehaviour>)GameOptionsExtension.UpdateSetting;
					}
					else if (setting.settingtype == SettingType.StringList)
					{
						StringOption opt = GameObject.Instantiate<StringOption>(stringoption, toggleoption.transform.parent);
						opt.transform.localPosition = new Vector3(opt.transform.localPosition.x, start_y - (0.5f * i), opt.transform.localPosition.z);
						opt.Title = (StringNames)i; //clever yet stupid way of checking what setting it is
						opt.TitleText.text = setting.display_name;
						opt.ValueText.text = (setting as StringListSetting).Strings[(byte)setting.Value];
						opt.Value = (byte)setting.Value;
						opt.OnValueChanged = (Action<OptionBehaviour>)GameOptionsExtension.UpdateSetting;
					}
				}


				//DebugLog.ShowMessage("max:" + __instance.GetComponentInParent<Scroller>().YBounds.max / (__instance.GetComponentsInChildren<OptionBehaviour>().Length - 2));

				__instance.Children = (UnhollowerBaseLib.Il2CppReferenceArray<OptionBehaviour>)__instance.GetComponentsInChildren<OptionBehaviour>();
				__instance.cachedData = PlayerControl.GameOptions;
				for (int i = 0; i < __instance.Children.Length; i++)
				{
					OptionBehaviour optionBehaviour = __instance.Children[i];
					if ((int)optionBehaviour.Title > AmongUsCEDE.MaxSettingAmount)
					{
						optionBehaviour.OnValueChanged = new Action<OptionBehaviour>(__instance.ValueChanged);
					}
					if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
					{
						optionBehaviour.SetAsPlayer();
					}
				}
				scroller = __instance.GetComponentInParent<Scroller>();
				return false;


			}

		}


		[HarmonyPatch(typeof(CustomPlayerMenu))]
		[HarmonyPatch("Start")]
		class NoShowForNonHost //TODO: actually fix GameOptionsMenuPatch instead of this stupid work around
		{
			static void Prefix(CustomPlayerMenu __instance)
			{
				if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
				{
					GameObject DESTROY = GameObject.Find("GameTab");
					if (DESTROY == null) return;
					GameObject.Destroy(DESTROY);
				}
			}
		}


		[HarmonyPatch(typeof(GameOptionsMenu))]
		[HarmonyPatch("Update")]
		class GameOptionsMenuUpdatePatch
		{
			static void Postfix(GameOptionsMenu __instance)
			{
				float length = __instance.Children.Length * 0.5f;
				if (scroller != null)
				{
					if (scroller.YBounds.max != length)
					{
						scroller.YBounds.max = length;
					}
				}
			}
		}
	}
}
