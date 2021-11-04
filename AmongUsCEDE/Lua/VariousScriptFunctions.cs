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
using AmongUsCEDE.Core;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using MoonSharp.Interpreter;
using AmongUsCEDE.Core.CustomSettings;

namespace AmongUsCEDE.Mods
{


	public static class VariousScriptFunctions
	{
		private static DynValue TryGet(this Table tab, string key, DynValue defaul, bool forcesametype = true)
		{
			DynValue val = tab.Get(key);
			if (val == null) return defaul;
			if (val.Type != DataType.Nil && (val.Type == defaul.Type || !forcesametype))
			{
				return val;
			}
			else
			{
				UnityEngine.Debug.Log("key:" + key + "\nval is null:" + (val == null) + "\ntype:" + val.Type + "\n" + defaul.Type);
				return defaul;
			}
		}

		public static void SendToHost(byte id, bool important, List<DynValue> values)
		{
			GameFunctions.SendCustomHostRequestRPC(PlayerControl.LocalPlayer,id, important, values);
		}


		public static void AddIntSetting(string intern_name, string display_name, string suffix, int def, int increment, int min, int max)
		{
			ModLoader.TempSettings.Add(new Setting(intern_name, display_name, suffix, SettingType.Int, def, increment, min, max));
		}

		public static void AddFloatSetting(string intern_name, string display_name, string suffix, float def, float increment, float min, float max)
		{
			ModLoader.TempSettings.Add(new Setting(intern_name, display_name, suffix, SettingType.Float, def, increment, min, max));
		}

		public static void AddToggleSetting(string intern_name, string display_name, bool value, string[] defstring)
		{
			ModLoader.TempSettings.Add(new BoolSetting(intern_name, display_name, value, defstring));
		}

		public static void AddStringListSetting(string intern_name, string display_name, byte defau, string[] stringstuffs)
		{
			ModLoader.TempSettings.Add(new StringListSetting(intern_name,display_name, (byte)((int)defau - 1),stringstuffs));
		}

		public static bool AmHost()
		{
			if (!AmongUsClient.Instance) return false;
			return AmongUsClient.Instance.AmHost;
		}


		public static float GetInternalNumberSetting(string setting_name)
		{
			switch (setting_name)
			{
				case "impostor_count":
					return PlayerControl.GameOptions.NumImpostors;
				case "crewmate_vision":
					return PlayerControl.GameOptions.CrewLightMod;
				case "impostor_vision":
					return PlayerControl.GameOptions.ImpostorLightMod;
				case "max_players":
					return PlayerControl.GameOptions.MaxPlayers;
				default:
					break;
			}
			return -1f;
		}


		public static float GetNumberSetting(string internal_name)
		{
			Setting setting = ScriptManager.CurrentGamemode.Settings.Find(s => s.internal_name == internal_name);
			if (setting == null) return -1f;
			if (setting.settingtype == SettingType.Float || setting.settingtype == SettingType.Int || setting.settingtype == SettingType.StringList)
			{
				if (setting.settingtype == SettingType.Float) //TODO: figure out if this is necessary
				{
					return (float)setting.Value;
				}
				else if (setting.settingtype == SettingType.Int)
				{
					return (int)setting.Value;
				}
				else
				{
					return (byte)setting.Value + 1;
				}
			}
			return -1f;
		}

		public static bool GetToggleSetting(string internal_name)
		{
			Setting setting = ScriptManager.CurrentGamemode.Settings.Find(s => s.internal_name == internal_name);
			if (setting.settingtype == SettingType.Toggle)
			{
				return (bool)setting.Value;
			}
			return false;
		}


		public static void AddHookLua(string hook, Closure function)
		{
			ModLoader.TempHooks.Add(hook,new CodeHook(ScriptLanguage.Lua,function));
		}

		public static void AddRoleLua(Table parms)
		{
			Role role = new Role(parms.Get("internal_name").String, parms.Get("name").String);
			role.Reveal_Text = parms.Get("role_text").String;
			Table specialtable = parms.Get("specials").Table;
			for (int i = 1; i < specialtable.Length + 1; i++)
			{
				role.AvailableSpecials.Add((RoleSpecials)specialtable.Get(i).Number);
			}
			role.FakeTaskString = parms.TryGet("task_text", DynValue.NewString("Define task_text please lol")).String;
			role.Visibility = (RoleVisibility)parms.TryGet("role_vis", DynValue.NewNumber(0)).Number;
			role.HasTasks = parms.TryGet("has_tasks", DynValue.True).Boolean;
			role.ImmuneToAffectors = parms.TryGet("immune_to_light_affectors", DynValue.False).Boolean;
			UnityEngine.Debug.Log(role.HasTasks.ToString());
			role.Layer = (byte)parms.TryGet("layer", DynValue.NewNumber(255)).Number;
			role.Team = (byte)parms.TryGet("team", DynValue.NewNumber(0)).Number;
			DynValue colorval = parms.TryGet("color", DynValue.NewNil(), false);
			Color color = Palette.ClearWhite;
			if (colorval.Type == DataType.Table)
			{
				Table colortable = colorval.Table;
				color = new Color32((byte)colortable.Get("r").Number, (byte)colortable.Get("g").Number, (byte)colortable.Get("b").Number, 255);
			}
			role.RoleColor = color;

			DynValue textcolorval = parms.TryGet("name_color", DynValue.NewNil(), false);
			Color textcolor = color;
			if (textcolorval.Type == DataType.Table)
			{
				Table textcolortable = textcolorval.Table;
				textcolor = new Color32((byte)textcolortable.Get("r").Number, (byte)textcolortable.Get("g").Number, (byte)textcolortable.Get("b").Number, 255);
			}
			role.RoleTextColor = textcolor;

			DynValue targetvalue = parms.TryGet("primary_valid_targets", DynValue.NewNumber(0));

			role.PrimaryTargets = (PrimaryTarget)targetvalue.Number;

			UnityEngine.Debug.Log(role.ToString());

			ModLoader.TempRoles.Add(role);

		}

		public static void MurderPlayer(PlayerInfoLua user, PlayerInfoLua target, bool DoAnimation)
		{
			if (!AmongUsClient.Instance.AmHost) return;
			GameFunctions.WriteMurderButBetter(PlayerControl.LocalPlayer, user.refplayer.Object, target.refplayer.Object, DoAnimation);
		}

		public static void WinGame(List<PlayerInfoLua> players, string sound)
		{
			if (!AmongUsClient.Instance.AmHost) return;
			List<GameData.PlayerInfo> fos = new List<GameData.PlayerInfo>();
			for (int i = 0; i < players.Count; i++)
			{
				fos.Add(players[i].refplayer);
			}
			GameFunctions.WriteVictory(fos, sound,VictoryTypes.Vanilla);
		}

		public static void WinAltGame(string type)
		{
			VictoryTypes tp = VictoryTypes.Error;
			switch (type)
			{
				case "stalemate":
					tp = VictoryTypes.Stalemate;
					break;
				case "tie":
					tp = VictoryTypes.Tie;
					break;
				case "error":
					break;
				default:
					UnityEngine.Debug.Log("Invalid VictoryType. Cancelling game with Error.");
					break;
			}
			GameFunctions.WriteVictory(new List<GameData.PlayerInfo>(), "hardcoded", tp);

		}


		public static List<PlayerInfoLua> GetAllPlayers(bool MustBeAlive)
		{
			List<PlayerInfoLua> folist = new List<PlayerInfoLua>();
			foreach (GameData.PlayerInfo fo in GameData.Instance.AllPlayers)
			{
				if (MustBeAlive && fo.IsDead) continue;
				folist.Add((PlayerInfoLua)fo);
			}
			return folist;
		}

		public static List<PlayerInfoLua> GetAllOnLayer(byte Layer, bool MustBeAlive)
		{
			List<PlayerInfoLua> folist = new List<PlayerInfoLua>();
			foreach (GameData.PlayerInfo fo in GameData.Instance.AllPlayers)
			{
				if (MustBeAlive && fo.IsDead) continue;
				if (fo.GetRole().Layer == Layer)
				{
					folist.Add((PlayerInfoLua)fo);
				}
			}
			return folist;
		}

		public static List<PlayerInfoLua> GetAllOnTeam(byte Team, bool MustBeAlive)
		{
			List<PlayerInfoLua> folist = new List<PlayerInfoLua>();
			foreach (GameData.PlayerInfo fo in GameData.Instance.AllPlayers)
			{
				if (MustBeAlive && fo.IsDead) continue;
				if (fo.GetRole().Team == Team)
				{
					folist.Add((PlayerInfoLua)fo);
				}
			}
			return folist;
		}


	}
}
