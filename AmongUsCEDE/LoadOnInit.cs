using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.Text;
using System.IO;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
//more stuff
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using AmongUsCEDE.Mods;
using AmongUsCEDE.Core.Extensions;

namespace AmongUsCEDE
{
	[HarmonyPatch(typeof(VersionShower))]
	[HarmonyPatch("Start")]
	class TextPatch
	{

		public static void Postfix(VersionShower __instance)
		{
			DestroyableSingleton<ModManager>.Instance.ShowModStamp();
			__instance.text.text = "Among Us: CE, DEV VERSION 0";
			for (int i = 0; i < Palette.PlayerColors.Length; i++)
			{
				CustomPalette.PlayerColors.Add(new PlayerColor(Palette.PlayerColors[i], Palette.ShadowColors[i], Palette.ColorNames[i].ToString().Remove(0, 5)));
			}
			if (!CEManager.ModsLoaded)
			{
				ModLoader.LoadMods();
			}

			PlayerInfoExtensions.FlushAllExtensions(); //adds all PlayerInfoExtension stuff

			__instance.text.text += "\nMods:" + ModLoader.Mods.Count;

			GameObject free = GameObject.Find("FreePlayButton");
			if (free != null)
			{
				GameObject.Destroy(free);
			}

			GameObject how = GameObject.Find("HowToPlayButton");
			if (how != null)
			{
				GameObject.Destroy(how);
			}
		}
	}
}
