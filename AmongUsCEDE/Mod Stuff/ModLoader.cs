using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using AmongUsCEDE.Extensions;
using AmongUsCEDE.Core;
using AmongUsCEDE.LuaData;
using MoonSharp.Interpreter;
using AmongUsCEDE.Core.CustomSettings;

namespace AmongUsCEDE.Mods
{
	public static class ModLoader
	{
		public const int FileVersion = 1;

		public static List<Mod> Mods = new List<Mod>();


		public static void UpdateDisabledMods()
		{
			string disablednames = "";
			foreach (Mod mod in Mods)
			{
				if (!mod.Enabled)
				{
					disablednames += (mod.ModName + "\n");
				}
			}
			File.WriteAllText(Path.Combine(CEExtensions.GetGameDirectory(), "disabledmods.txt"), disablednames);

		}

		public static bool LoadMods()
		{
			string AttemptedDir = Path.Combine(CEExtensions.GetGameDirectory(), "Mods");

			string[] DisabledModNames = new string[0];

			char[] split = new char[2]
					{
						'\r','\n'
					};
			if (File.Exists(Path.Combine(CEExtensions.GetGameDirectory(), "disabledmods.txt")))
			{
				DisabledModNames = File.ReadAllText(Path.Combine(CEExtensions.GetGameDirectory(), "disabledmods.txt")).Split(split, StringSplitOptions.RemoveEmptyEntries);
			}
			else
			{
				File.WriteAllText(Path.Combine(CEExtensions.GetGameDirectory(), "disabledmods.txt"), "");
			}
			if (Directory.Exists(AttemptedDir))
			{
				foreach (string DS in Directory.GetDirectories(AttemptedDir))
				{
					string[] txts = Directory.GetFiles(DS, "modinfo.json");
					if (txts.Length != 1)
					{
						continue;
					}

					Mod CURM = JsonConvert.DeserializeObject<Mod>(File.ReadAllText(Path.Combine(DS, "modinfo.json")));
					CURM.BaseDirectory = DS;
					Mods.Add(CURM);
					if (DisabledModNames.Any(CURM.ModName.Contains))
					{
						CURM.Enabled = false;
						continue;
					}
					if (Directory.Exists(Path.Combine(DS, "Scripts")))
					{
						if (Directory.Exists(Path.Combine(DS, "Scripts", "Gamemodes")))
						{
							string luapath = Path.Combine(DS, "Scripts", "Gamemodes");

							foreach (string luafile in Directory.GetFiles(luapath, "*.lua"))
							{
								LoadGamemode(luafile, ref CURM);
							}
						}
					}

					if (Directory.Exists(Path.Combine(DS, "Resources")))
					{
						if (File.Exists(Path.Combine(DS, "Resources", "colors.json")))
						{
							if (!AmongUsCEDE.Feature_Enabled(CE_Features.CustomColors)) continue;
							CustomPalette.PlayerColors.AddRange(JsonConvert.DeserializeObject<List<PlayerColor>>(File.ReadAllText(Path.Combine(DS, "Resources", "colors.json"))));
						}
					}
				}
			}
			else
			{
				Application.Quit(2);
			}

			CEManager.ModsLoaded = true;
			return true;
		}

		public static List<Role> TempRoles = new List<Role>();

		public static Dictionary<string, CodeHook> TempHooks = new Dictionary<string, CodeHook>();

		public static List<Setting> TempSettings = new List<Setting>(); //incase anyone asks, this is just to make sure settings always land up in the same place


		public static void AddEnumToLuaScript(Script scr, string prefix, Type num)
		{
			string[] numnames = Enum.GetNames(num);
			for (int i = 0; i < numnames.Length; i++)
			{
				scr.Globals[prefix + numnames[i]] = i;
			}
		}


		public static bool LoadGamemode(string path, ref Mod ModToAddTo)
		{
			string text = "\n" + File.ReadAllText(path);
			Script script = new Script(CoreModules.Preset_HardSandbox & CoreModules.OS_Time); //this makes it way more secure then before. adding OS_TIME cus i dont see any reason why not and someone may want it
			CodeScript cscript = new CodeScript(ScriptLanguage.Lua);
			cscript.Script = script;
			AddData(cscript, ScriptType.Gamemode, ScriptLanguage.Lua, true);
			AddEnumToLuaScript(script, "RS_", typeof(RoleSpecials));
			AddEnumToLuaScript(script, "RV_", typeof(RoleVisibility));
			AddEnumToLuaScript(script, "VPT_", typeof(PrimaryTarget));
			script.DoString(text);
			DynValue vals = script.Call(script.Globals["InitializeGamemode"]);
			if (vals.Type != DataType.Table) return false;
			Gamemode GM = new Gamemode(vals.Table.Get(2).String, vals.Table.Get(1).String);
			GM.Script = cscript;
			GM.Roles = TempRoles;
			GM.Hooks = TempHooks;
			GM.Settings = TempSettings;
			TempRoles = new List<Role>();
			TempHooks = new Dictionary<string, CodeHook>();
			TempSettings = new List<Setting>();
			GM.Script.FileLocation = path;
			ModToAddTo.Gamemodes.Add(GM);

			return true;
		}

		private static void Print(string text)
		{
			DebugLog.ShowMessage("Lua:" + text);
		}

		private static void AddData(CodeScript scr, ScriptType type, ScriptLanguage lang, bool includeinit) //made this not stupid :sunglasses:
		{
			switch (lang)
			{
				case ScriptLanguage.Lua:
					Script script = scr.Script as Script;
					script.Globals["print"] = (Action<string>)Print;
					script.Globals["CE_WinGame"] = (Action<List<PlayerInfoLua>, string>)VariousScriptFunctions.WinGame;
					script.Globals["CE_WinGameAlt"] = (Action<string>)VariousScriptFunctions.WinAltGame;
					script.Globals["CE_MurderPlayer"] = (Action<PlayerInfoLua, PlayerInfoLua, bool>)VariousScriptFunctions.MurderPlayer;
					script.Globals["CE_GetAllPlayers"] = (Func<bool, List<PlayerInfoLua>>)VariousScriptFunctions.GetAllPlayers;
					script.Globals["CE_GetAllPlayersOnLayer"] = (Func<byte, bool, List<PlayerInfoLua>>)VariousScriptFunctions.GetAllOnLayer;
					script.Globals["CE_GetAllPlayersOnTeam"] = (Func<byte, bool, List<PlayerInfoLua>>)VariousScriptFunctions.GetAllOnTeam;
					script.Globals["CE_GetNumberSetting"] = (Func<string, float>)VariousScriptFunctions.GetNumberSetting;
					script.Globals["CE_GetBoolSetting"] = (Func<string, bool>)VariousScriptFunctions.GetToggleSetting;
					script.Globals["CE_GetInternalNumberSetting"] = (Func<string, float>)VariousScriptFunctions.GetInternalNumberSetting;
					if (includeinit)
					{
						script.Globals["CE_AddRole"] = (Action<Table>)VariousScriptFunctions.AddRole;
						script.Globals["CE_AddIntSetting"] = (Action<string, string, string, int, int, int, int>)VariousScriptFunctions.AddIntSetting;
						script.Globals["CE_AddFloatSetting"] = (Action<string, string, string, float, float, float, float>)VariousScriptFunctions.AddFloatSetting;
						script.Globals["CE_AddToggleSetting"] = (Action<string, string, bool, string[]>)VariousScriptFunctions.AddToggleSetting;
						script.Globals["CE_AddStringSetting"] = (Action<string, string, byte, string[]>)VariousScriptFunctions.AddStringListSetting;
						script.Globals["CE_AddHook"] = (Action<string,Closure>)VariousScriptFunctions.AddHookLua;
					}
					else
					{
						script.Globals["CE_AddRole"] = null;
						script.Globals["CE_AddHook"] = null;
						script.Globals["CE_AddIntSetting"] = null;
						script.Globals["CE_AddFloatSetting"] = null;
						script.Globals["CE_AddToggleSetting"] = null;
						script.Globals["CE_AddStringSetting"] = null;
					}
					break;
			}
		}
	
	}




}
