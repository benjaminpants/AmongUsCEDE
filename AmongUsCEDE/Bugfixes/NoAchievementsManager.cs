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


//only compile this work around for debug builds, so multiple instances don't break
#if DEBUG

namespace AmongUsCEDE
{
	internal class NoMoreAchievements
	{

		[HarmonyPatch(typeof(AchievementManager))]
		[HarmonyPatch("OnTaskComplete")]
		class NoOnTaskComplete
		{
			public static bool Prefix()
			{
				return false;
			}
		}

	}

}

#endif