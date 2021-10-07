using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.CustomSettings;

namespace AmongUsCEDE.Core
{
	public static class GameOptionsExtension
	{
		public static int Gamemode = 0;


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
