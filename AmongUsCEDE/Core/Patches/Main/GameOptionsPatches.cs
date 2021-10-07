using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.Core.CustomSettings;
using BepInEx;
using HarmonyLib;
using System.IO;
using InnerNet;
using Hazel;
using PowerTools;
using UnityEngine;
using System.Linq;

namespace AmongUsCEDE.Core
{
	internal class GameOptionsPatches
	{
		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("Deserialize", new Type[] { typeof(MessageReader) })]
		class DeserializePatch
		{
			static void Postfix(ref MessageReader reader, ref GameOptionsData __result)
			{
				if (__result == null)
				{
					GameOptionsExtension.ReturnToDefault();
					return;
				}
				GameOptionsExtension.Gamemode = reader.ReadInt32();
				for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
				{
					switch (ScriptManager.CurrentGamemode.Settings[i].settingtype)
					{
						case SettingType.Int:
							ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadInt32();
							break;
						case SettingType.Float:
							ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadSingle();
							break;
						case SettingType.Toggle:
							ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadBoolean();
							break;
						default:
							throw new NotImplementedException(ScriptManager.CurrentGamemode.Settings[i].settingtype + " not implemented! (GameOptionsData Deserialize)");
					}

				}
				return;

			}
		}

		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("ToBytes")]
		class ToBytesPatch
		{
			static void Postfix(byte version, UnhollowerBaseLib.Il2CppStructArray<byte> __result)
			{
				byte[] results;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter writer = new BinaryWriter(memoryStream))
					{
						writer.Write(GameOptionsExtension.Gamemode);
						for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
						{
							ScriptManager.CurrentGamemode.Settings[i].Serialize(writer);
						}
						//aaa
						writer.Flush();
						memoryStream.Position = 0L;
						results = memoryStream.ToArray();
					}
				}
				UnhollowerBaseLib.Il2CppStructArray<byte> result = new UnhollowerBaseLib.Il2CppStructArray<byte>(results.Length + __result.Length);
				for (int i = 0; i < results.Length; i++)
				{
					result[i] = __result[i];
				}
				for (int i = __result.Length; i < results.Length; i++)
				{
					result[i] = results[i];
				}
				__result = result;
			}
		}

		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("ToHudString")]
		class HudStringPatch
		{
			static void Postfix(ref string __result)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Gamemode: " + ScriptManager.CurrentGamemode.DisplayName);
				for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
				{
					builder.AppendLine(ScriptManager.CurrentGamemode.Settings[i].ToString());
				}
				__result += builder.ToString();
			}
		}



		[HarmonyPatch(typeof(GameOptionsData))]
		[HarmonyPatch("FromBytes")]
		class FromBytesPatch
		{
			static bool Prefix(UnhollowerBaseLib.Il2CppStructArray<byte> bytes, out int __state)
			{
				GameOptionsData result;
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						result = (GameOptionsData.Deserialize(new System.IO.BinaryReader(memoryStream)) ?? new GameOptionsData());
					}
				}
				__state = 0;
				return false;
			}
			static void Postfix(UnhollowerBaseLib.Il2CppStructArray<byte> bytes)
			{
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					using (BinaryReader reader = new BinaryReader(memoryStream))
					{
						GameOptionsExtension.Gamemode = reader.ReadInt32();
						for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
						{
							switch (ScriptManager.CurrentGamemode.Settings[i].settingtype)
							{
								case SettingType.Int:
									ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadInt32();
									break;
								case SettingType.Float:
									ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadSingle();
									break;
								case SettingType.Toggle:
									ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadBoolean();
									break;
								default:
									throw new NotImplementedException(ScriptManager.CurrentGamemode.Settings[i].settingtype + " not implemented! (GameOptionsData Deserialize)");
							}

						}
					}
				}
			}
		}

	}


	//[HarmonyPatch(typeof(SaveManager))]
	//[HarmonyPatch("SaveGameOptions")]
	class SaveGameOptionsPatch
	{
		static bool Prefix(GameOptionsData data, ref string filename, bool saveNow)
		{
			if (filename.StartsWith(AmongUsCEDE.CustomDataPrefix)) return true;
			filename = AmongUsCEDE.CustomDataPrefix + filename;
			SaveManager.SaveGameOptions(data,filename,saveNow);
			return false;
		}
	}

	//[HarmonyPatch(typeof(SaveManager))]
	//[HarmonyPatch("LoadGameOptions")]
	class LoadGameOptionsPatch
	{
		static bool Prefix(ref string filename)
		{
			if (filename.StartsWith(AmongUsCEDE.CustomDataPrefix)) return true;
			filename = AmongUsCEDE.CustomDataPrefix + filename;
			SaveManager.LoadGameOptions(filename);
			return false;
		}
	}

}
