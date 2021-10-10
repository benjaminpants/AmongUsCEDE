using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using BepInEx.Logging;
using MoonSharp.Interpreter;
using UnityEngine.SceneManagement;
using Consensus; //allows patching for ienumerators
using AmongUsCEDE.Mods;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.Utilities;

namespace AmongUsCEDE
{
	[BepInPlugin("mtm101.rulerp.moogus.amongusce")]
	[BepInProcess("Among Us.exe")]
	public class AmongUsCEDE : BasePlugin
	{
		public Harmony Harmony { get; } = new Harmony("mtm101.rulerp.moogus.amongusce");
		
		public const int Lua_UserDataAmount = 4; //how many "UserData" variables are available to use. Beware increasing this increases packet size!

		public const int MaxSettingAmount = 128; //this isn't exactly a limit, more of the index before conflicts with StringName start occuring

		public const string CustomDataPrefix = "cede_";

		private void LoadMods()
		{
			if (!CEManager.ModsLoaded)
			{
				for (int i = 0; i < Palette.PlayerColors.Length; i++)
				{
					CustomPalette.PlayerColors.Add(new PlayerColor(Palette.PlayerColors[i], Palette.ShadowColors[i], Palette.ColorNames[i].ToString().Remove(0, 5)));
				}
				ModLoader.LoadMods();
			}

			PlayerInfoExtensions.FlushAllExtensions(); //adds all PlayerInfoExtension stuff
		}

		public override void Load()
		{
			ConsensusPatcher.Patch(Harmony);
			UserData.RegisterAssembly();
			SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>)((scene, loadscenemode) =>
			{
				if (scene.name == "SplashIntro")
				{
					ResourcesManager.LoadAllTextures();
					LoadMods();
				}
			}));
			Harmony.PatchAll();
		}
	}

}
