using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using BepInEx.Logging;
using MoonSharp.Interpreter;
using Consensus; //allows patching for ienumerators

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
		public override void Load()
		{
			ConsensusPatcher.Patch(Harmony);
			UserData.RegisterAssembly();
			Harmony.PatchAll();
		}
	}

}
