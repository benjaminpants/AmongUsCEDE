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
		const int page_size = 24; //subtract by 4 for buttons, true page size is 28

		static bool Prefix(PlayerTab __instance)
		{
			CEManager.Select_Chips = new List<ColorChip>();
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
			for (int i = (CEManager.Colors_Page * (page_size - 1)); i < max_amount; i++)
			{
				int i_modded = i % (page_size + 1);
				float x = __instance.XRange.Lerp((float)(i_modded % 4) / 3f);
				float y = __instance.YStart - (float)(i_modded / 4) * num2;
				ColorChip colorChip = UnityEngine.Object.Instantiate<ColorChip>(__instance.ColorTabPrefab);
				colorChip.transform.SetParent(__instance.ColorTabArea);
				colorChip.transform.localPosition = new Vector3(x, y, -1f);
				int j = i;
				colorChip.Button.OnClick.AddListener((Action)delegate
				{
					__instance.SelectColor(j);
				});
				colorChip.Inner.color = CustomPalette.PlayerColors[i].Base;
				__instance.ColorChips.Add(colorChip);
			}
			for (int i = 0; i < 5; i++) //this is incomplete, lol
			{
				int i_modded = i + page_size;
				float x = __instance.XRange.Lerp((float)(i_modded % 4) / 3f);
				float y = __instance.YStart - (float)(i_modded / 4) * num2;
				ColorChip colorChip = UnityEngine.Object.Instantiate<ColorChip>(__instance.ColorTabPrefab);
				colorChip.transform.SetParent(__instance.ColorTabArea);
				colorChip.transform.localPosition = new Vector3(x, y, -1f);
				int j = i;
				
				Texture2D tex = ResourcesManager.GetTexture("colorchip_special.png", "");
				DebugLog.ShowMessage("x" + colorChip.Inner.BackLayer.sprite.texture.width);
				DebugLog.ShowMessage("y" + colorChip.Inner.BackLayer.sprite.texture.height);
				Sprite torture = Sprite.Create(tex, new Rect(0f, 0f, 43f, 43f), Vector2.zero);
				colorChip.Inner.BackLayer.sprite = torture;
				colorChip.Inner.FrontLayer.sprite = torture;
				colorChip.Inner.color = Color.white;
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(PlayerTab))]
	[HarmonyPatch("UpdateAvailableColors")]
	class ColorUpdate
	{
		static bool Prefix(PlayerTab __instance)
		{
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
