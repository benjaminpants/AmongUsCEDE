using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.Events;
//BepInEx stuff
using BepInEx;
using UnityEngine;
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using AmongUsCEDE.HelperExtensions;
using AmongUsCEDE.Utilities;
using AmongUsCEDE.Core;

namespace AmongUsCEDE.Colors
{
	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("OnEnable")]
	class ColorMenuPatch
	{
		public const int page_size = 24; //subtract by 4 for buttons, true page size is 28

		public static List<ColorChip> arrow_chips = new List<ColorChip>();


		public static void SetPage(PlayerTab instance, int page)
		{
			CEManager.Colors_Page = page;
			int max_page = Mathf.CeilToInt((float)CustomPalette.PlayerColors.Count / (float)page_size) - 1;
			if (CEManager.Colors_Page > max_page)
			{
				CEManager.Colors_Page = max_page;
			}
			else if (CEManager.Colors_Page < 0)
			{
				CEManager.Colors_Page = 0;
			}
			instance.OnDisable();
			instance.OnEnable();
		}

		public static void ChangePage(PlayerTab instance, int page)
		{
			CEManager.Colors_Page += page;
			int max_page = Mathf.CeilToInt((float)CustomPalette.PlayerColors.Count / (float)page_size) - 1;
			if (CEManager.Colors_Page > max_page)
			{
				CEManager.Colors_Page = max_page;
			}
			else if (CEManager.Colors_Page < 0)
			{
				CEManager.Colors_Page = 0;
			}
			instance.OnDisable();
			instance.OnEnable();
		}



		static bool Prefix(PlayerTab __instance)
		{
			if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) return true;
			if (arrow_chips.Count != 0)
			{
				for (int i = 0; i < arrow_chips.Count; i++)
				{
					GameObject.Destroy(arrow_chips[i]);
				}
				arrow_chips = new List<ColorChip>();
			}
			int max_page = Mathf.CeilToInt((float)CustomPalette.PlayerColors.Count / (float)page_size) - 1;
			if (CEManager.Colors_Page > max_page)
			{
				CEManager.Colors_Page = max_page;
			}
			else if (CEManager.Colors_Page < 0)
			{
				CEManager.Colors_Page = 0;
			}
			int max_amount = ((CEManager.Colors_Page * page_size) + page_size > CustomPalette.PlayerColors.Count) ? CustomPalette.PlayerColors.Count : (CEManager.Colors_Page * page_size) + page_size;
			PlayerControl.SetPlayerMaterialColors(PlayerControl.LocalPlayer.Data.ColorId, __instance.DemoImage);
			__instance.HatImage.SetHat(SaveManager.LastHat, PlayerControl.LocalPlayer.Data.ColorId);
			PlayerControl.SetSkinImage(SaveManager.LastSkin, __instance.SkinImage);
			PlayerControl.SetPetImage(SaveManager.LastPet, PlayerControl.LocalPlayer.Data.ColorId, __instance.PetImage);
			float num = (float)CustomPalette.PlayerColors.Count / 4f;
			float num2 = 0.55f;
			for (int i = (CEManager.Colors_Page * (page_size - 1)); i < max_amount; i++) //WHY THE FUCK IS THERE A STRAY COLOR WHEN PAGES SWITCH???
			{
				int i_modded = i % (page_size);
				float x = __instance.XRange.Lerp((float)(i_modded % 4) / 3f);
				float y = __instance.YStart - (float)(i_modded / 4) * num2;
				ColorChip colorChip = UnityEngine.Object.Instantiate<ColorChip>(__instance.ColorTabPrefab);
				colorChip.transform.SetParent(__instance.ColorTabArea);
				colorChip.transform.localPosition = new Vector3(x, y, -1f);
				UnityEngine.Debug.Log("creating color:" + i);
				int j = i;
				colorChip.Button.OnClick.AddListener((Action)delegate
				{
					__instance.SelectColor(j);
				});
				colorChip.Inner.color = CustomPalette.PlayerColors[i].Base;
				__instance.ColorChips.Add(colorChip);
			}
			for (int i = 0; i < 4; i++) //this is incomplete, lol
			{
				int i_modded = i + page_size;
				float x = __instance.XRange.Lerp((float)(i_modded % 4) / 3f);
				float y = __instance.YStart - (float)(i_modded / 4) * num2;
				ColorChip colorChip = UnityEngine.Object.Instantiate<ColorChip>(__instance.ColorTabPrefab);
				colorChip.transform.SetParent(__instance.ColorTabArea);
				colorChip.transform.localPosition = new Vector3(x, y, -1f);
				string texture_to_grab = "colorchip_double_left.png";
				switch (i)
				{
					case 0:
						texture_to_grab = "colorchip_double_left.png";
						colorChip.Button.OnClick.AddListener((Action)delegate
						{
							SetPage(__instance,0);
						});
						break;
					case 1:
						texture_to_grab = "colorchip_left.png";
						colorChip.Button.OnClick.AddListener((Action)delegate
						{
							ChangePage(__instance, -1);
						});
						break;
					case 2:
						texture_to_grab = "colorchip_right.png";
						colorChip.Button.OnClick.AddListener((Action)delegate
						{
							ChangePage(__instance, 1);
						});
						break;
					case 3:
						texture_to_grab = "colorchip_double_right.png";
						colorChip.Button.OnClick.AddListener((Action)delegate
						{
							SetPage(__instance, max_page);
						});
						break;

				}
				
				Texture2D tex = ResourcesManager.GetTexture(texture_to_grab, "");
				Sprite torture = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f,0.5f));
				colorChip.Inner.BackLayer.sprite = torture;
				colorChip.Inner.FrontLayer.sprite = torture;
				colorChip.Inner.color = Color.white;

				arrow_chips.Add(colorChip);
			}
			return false;
		}
	}


	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("GetDefaultSelectable")]
	class DefaultSelectablePatch
	{
		static bool Prefix(PlayerTab __instance, ref ColorChip __result)
		{
			if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) return true;
			__result = __instance.ColorChips[PlayerControl.LocalPlayer.Data.ColorId % (ColorMenuPatch.page_size + 1)];
			return false;
		}
	}
	
	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("OnDisable")]
	class ColorDisablePatch
	{
		static void Postfix()
		{
			if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) return;
			for (int i = 0; i < ColorMenuPatch.arrow_chips.Count; i++)
			{
				GameObject.Destroy(ColorMenuPatch.arrow_chips[i]);
			}
			ColorMenuPatch.arrow_chips = new List<ColorChip>();
		}
	}

	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("Update")]
	class ColorUpdate
	{
		static bool Prefix(PlayerTab __instance)
		{
			if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) return true;
			__instance.UpdateAvailableColors();
			for (int i = 0; i < __instance.ColorChips.Count; i++)
			{
				__instance.ColorChips[i].InUseForeground.SetActive(!__instance.AvailableColors.Contains(i + CEManager.Colors_Page * (ColorMenuPatch.page_size - 1)));
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("UpdateAvailableColors")]
	class ColorAvailableUpdate
	{
		static bool Prefix(PlayerTab __instance)
		{
			if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) return true;
			PlayerControl.SetPlayerMaterialColors(PlayerControl.LocalPlayer.Data.ColorId, __instance.DemoImage);
			PlayerControl.SetPetImage(SaveManager.LastPet, PlayerControl.LocalPlayer.Data.ColorId, __instance.PetImage);
			for (int i = 0; i < CustomPalette.PlayerColors.Count; i++)
			{
				__instance.AvailableColors.Add(i);
			}
			if (GameData.Instance)
			{
				List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers.ToProperList();
				for (int j = 0; j < allPlayers.Count; j++)
				{
					GameData.PlayerInfo playerInfo = allPlayers[j];
					__instance.AvailableColors.Remove(playerInfo.ColorId);
				}
			}

			return false;
		}
	}
}
