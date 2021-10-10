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
