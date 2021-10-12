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

	[Flags]
	public enum CE_Features //an enum, containing which features should be enabled in this build of CE.
	{
		None = 0,
		CustomColors = 1,
		CustomHats = 2,
		LiveModDisables = 4
	}




	[BepInPlugin("mtm101.rulerp.moogus.amongusce")]
	[BepInProcess("Among Us.exe")]
	public class AmongUsCEDE : BasePlugin
	{
		public Harmony Harmony { get; } = new Harmony("mtm101.rulerp.moogus.amongusce");
		
		public const int Lua_UserDataAmount = 4; //how many "UserData" variables are available to use. Beware increasing this increases packet size!

		public const int MaxSettingAmount = 128; //this isn't exactly a limit, more of the index before conflicts with StringName start occuring

		public const string CustomDataPrefix = "cede_";

		public const CE_Features features = (CE_Features.CustomColors | CE_Features.CustomHats);

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

		public static bool Feature_Enabled(CE_Features feature)
		{
			return ((features & feature) == feature);
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
