// AmongUsCEDE.UI.Patches.EndGameManagerPatch
using System;
using System.Collections.Generic;
using System.Linq;
using AmongUsCEDE;
using AmongUsCEDE.HelperExtensions;
using Assets.CoreScripts;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(EndGameManager))]
[HarmonyPatch("SetEverythingUp")]
internal class EndGameManagerPatch
{
	private static bool Prefix(EndGameManager __instance)
	{
		StatsManager.Instance.GamesFinished++;
		if (CETempData.VictoryType != 0)
		{
			switch (CETempData.VictoryType)
			{
				case VictoryTypes.Stalemate:
					__instance.WinText.text = "Stalemate";
					__instance.BackgroundBar.material.SetColor("_Color", Palette.Orange);
					__instance.WinText.color = Palette.Orange;
					SoundManager.Instance.PlayDynamicSound("Stinger", __instance.DisconnectStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
					break;
				case VictoryTypes.Tie:
					__instance.WinText.text = "Tie";
					__instance.BackgroundBar.material.SetColor("_Color", Palette.Orange);
					__instance.WinText.color = Palette.Orange;
					SoundManager.Instance.PlayDynamicSound("Stinger", __instance.DisconnectStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
					break;
				case VictoryTypes.Error:
					__instance.WinText.text = "Error";
					__instance.BackgroundBar.material.SetColor("_Color", Palette.White);
					__instance.WinText.color = Palette.DisabledGrey;
					SoundManager.Instance.PlayDynamicSound("Stinger", __instance.DisconnectStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
					break;
			}
			return false;
		}
		if (TempData.winners.ToProperList().Any((WinningPlayerData h) => h.IsYou))
		{
			__instance.WinText.text = "Victory";
			__instance.BackgroundBar.material.SetColor("_Color", Palette.CrewmateBlue);
			WinningPlayerData winningPlayerData = TempData.winners.ToProperList().FirstOrDefault((WinningPlayerData h) => h.IsYou);
			if (winningPlayerData != null)
			{
				DestroyableSingleton<Telemetry>.Instance.WonGame(winningPlayerData.ColorId, winningPlayerData.HatId);
			}
		}
		else
		{
			__instance.WinText.text = "Defeat";
			__instance.WinText.color = Color.red;
		}
		if (CETempData.End_Song == "default_crewmate")
		{
			SoundManager.Instance.PlayDynamicSound("Stinger", __instance.CrewStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
		}
		else if (CETempData.End_Song == "default_impostor")
		{
			SoundManager.Instance.PlayDynamicSound("Stinger", __instance.ImpostorStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
		}
		else
		{
			if (!(CETempData.End_Song == "default_disconnect"))
			{
				throw new NotImplementedException("Custom Stingers have not been implemented yet!!");
			}
			SoundManager.Instance.PlayDynamicSound("Stinger", __instance.DisconnectStinger, loop: false, (Action<AudioSource, float>)__instance.GetStingerVol);
		}
		int num = Mathf.CeilToInt(7.5f);
		List<WinningPlayerData> list = (from b in TempData.winners.ToProperList()
			orderby b.IsYou ? (-1) : 0
			select b).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			WinningPlayerData winningPlayerData2 = list[i];
			int num2 = ((i % 2 != 0) ? 1 : (-1));
			int num3 = (i + 1) / 2;
			float t = (float)num3 / (float)num;
			float num4 = Mathf.Lerp(1f, 0.75f, t);
			float num5 = ((i == 0) ? (-8) : (-1));
			PoolablePlayer poolablePlayer = UnityEngine.Object.Instantiate(__instance.PlayerPrefab, __instance.transform);
			poolablePlayer.transform.localPosition = new Vector3(1f * (float)num2 * (float)num3 * num4, FloatRange.SpreadToEdges(-1.125f, 0f, num3, num), num5 + (float)num3 * 0.01f) * 0.9f;
			float num6 = Mathf.Lerp(1f, 0.65f, t) * 0.9f;
			Vector3 vector = new Vector3(num6, num6, 1f);
			poolablePlayer.transform.localScale = vector;
			if (winningPlayerData2.IsDead)
			{
				poolablePlayer.Body.sprite = __instance.GhostSprite;
				poolablePlayer.SetDeadFlipX(i % 2 == 0);
			}
			else
			{
				poolablePlayer.SetFlipX(i % 2 == 0);
			}
			if (!winningPlayerData2.IsDead)
			{
				poolablePlayer.SetSkin(winningPlayerData2.SkinId);
			}
			else
			{
				poolablePlayer.HatSlot.color = new Color(1f, 1f, 1f, 0.5f);
			}
			PlayerControl.SetPlayerMaterialColors(winningPlayerData2.ColorId, poolablePlayer.Body);
			poolablePlayer.HatSlot.SetHat(winningPlayerData2.HatId, winningPlayerData2.ColorId);
			PlayerControl.SetPetImage(winningPlayerData2.PetId, winningPlayerData2.ColorId, poolablePlayer.PetSlot);
			if (list.Count <= 4)
			{
				((Component)(object)poolablePlayer.NameText).gameObject.SetActive(value: false);
				continue;
			}
			poolablePlayer.NameText.text = winningPlayerData2.Name;
			poolablePlayer.NameText.color = CETempData.AdditionalWinningInfos[i].RoleColor;
			poolablePlayer.NameText.transform.localScale = Extensions.Inv(vector);
			GameObjectExtensions.SetLocalZ(poolablePlayer.NameText.transform, -15f);
		}
		return false;
	}
}
