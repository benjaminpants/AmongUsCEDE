using System;
using System.Collections.Generic;
using System.Text;
using Hazel;
using AmongUsCEDE.Core.CustomSettings;

namespace AmongUsCEDE.Core
{
	public static class GameOptionsExtension
	{
		public static int Gamemode = 0;


		public static void WriteGameOptionsRPC(PlayerControl player, GameOptionsData gameOptions)
		{
			if (!AmongUsClient.Instance.AmHost || DestroyableSingleton<TutorialManager>.InstanceExists)
			{
				return;
			}
			PlayerControl.GameOptions = gameOptions;
			SaveManager.GameHostOptions = gameOptions;
			MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(player.NetId, 2, SendOption.Reliable);
			messageWriter.WriteBytesAndSize(gameOptions.ToBytes(4));
			messageWriter.Write(Gamemode);
			for (int i = 0; i < ScriptManager.CurrentGamemode.Settings.Count; i++)
			{
				ScriptManager.CurrentGamemode.Settings[i].Serialize(ref messageWriter);
			}
			messageWriter.EndMessage();
		}

		public static void Deserialize(MessageReader reader)
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
					case SettingType.StringList:
						ScriptManager.CurrentGamemode.Settings[i].Value = reader.ReadByte();
						break;
					default:
						throw new NotImplementedException(ScriptManager.CurrentGamemode.Settings[i].settingtype + " not implemented! (GameOptionsData Deserialize)");
				}

			}
		}



		public static void UpdateSetting(OptionBehaviour behav)
		{
			Setting customsetting = ScriptManager.CurrentGamemode.Settings[(int)behav.Title];
			switch (customsetting.settingtype)
			{
				case SettingType.Float:
					customsetting.Value = behav.GetFloat();
					break;
				case SettingType.Int:
					customsetting.Value = behav.GetInt();
					break;
				case SettingType.Toggle:
					customsetting.Value = behav.GetBool();
					break;
				case SettingType.StringList:
					customsetting.Value = (byte)behav.GetInt();
					break;
			}

			if (PlayerControl.LocalPlayer)
			{
				PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);
			}
		}


		public static void ReturnToDefault()
		{
			Gamemode = 0;
		}

	}
}
