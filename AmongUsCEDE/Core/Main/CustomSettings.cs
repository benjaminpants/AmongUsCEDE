using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Hazel;

namespace AmongUsCEDE.Core.CustomSettings
{

	public enum SettingType
	{
		Invalid,
		Int,
		Float,
		Toggle,
		StringList
	}



	public class Setting
	{
		public string internal_name = "internal_setting_name";
		public string display_name = "SUS SETTING";
		public string addend = "";
		public float Increment = 1f;

		public object Value;

		public object Default;

		public object Min;

		public object Max;

		public SettingType settingtype = SettingType.Invalid;


		public void Serialize(ref MessageWriter writer) //im going to buy fortnite season 6 battle pass
		{
			//writer.Write((byte)settingtype);
			//dont
			switch (settingtype)
			{
				case SettingType.Toggle:
					writer.Write((bool)Value);
					break;
				case SettingType.Float:
					writer.Write((float)Value);
					break;
				case SettingType.Int:
					writer.Write((int)Value);
					break;
				case SettingType.StringList:
					writer.Write((byte)Value);
					break;
			}
		}

		public Setting()
		{

		}

		public Setting(string internl, string externl, string adden, SettingType type, object defaul, float incr, object min, object max)
		{
			internal_name = internl;
			display_name = externl;
			addend = adden;
			settingtype = type;
			Value = defaul;
			Default = defaul;
			Increment = incr;
			Min = min;
			Max = max;
		}


		public override string ToString()
		{
			return display_name + ": " + Value.ToString() + addend;
		}
	}


	public class BoolSetting : Setting
	{
		public string[] ToggleStrings = new string[] { "True", "False" };

		public BoolSetting(string internl, string externl, bool def, string[] togstrings)
		{
			internal_name = internl;
			display_name = externl;
			Default = def;
			Value = def;
			ToggleStrings = togstrings;
			settingtype = SettingType.Toggle;
		}

		public override string ToString()
		{
			return display_name + ": " + ToggleStrings[((bool)Value) ? 0 : 1];
		}
	}

	public class StringListSetting : Setting
	{
		public string[] Strings = new string[1];

		public StringListSetting(string internl, string externl, byte defaul, string[] stringz)
		{
			internal_name = internl;
			display_name = externl;
			settingtype = SettingType.StringList;
			Value = defaul;
			Default = defaul;
			Increment = 1f;
			Min = 0;
			Max = stringz.Length - 1;
			Strings = stringz;
		}

		public override string ToString()
		{
			return display_name + ": " + Strings[((byte)Value)];
		}
	}


}
