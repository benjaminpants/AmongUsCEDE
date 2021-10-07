using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core.CustomSettings
{

	public enum SettingType
	{
		Invalid,
		Int,
		Float,
		Toggle
	}


	public class Setting
	{
		public string internal_name = "internal_setting_name";
		private string display_name = "SUS SETTING";
		private string addend = "";
		private float Increment = 1f;

		public object Value;

		public object Default;

		private object Min;

		private object Max;

		public SettingType settingtype = SettingType.Invalid;

		public void ChangeValue(bool Positive)
		{
			int mult = (Positive ? 1 : -1);

			switch (settingtype)
			{
				case SettingType.Toggle:
					Value = Positive;
					return;
				case SettingType.Int:
					Value = (int)Value - ((int)Increment * mult);
					break;
				case SettingType.Float:
					Value = (float)Value - (Increment * mult);
					break;
				default:
					throw new NotImplementedException(settingtype + " is not implemented!");
			}

			if ((float)Value < (float)Min)
			{
				Value = Min;
			}
			else if ((float)Value > (float)Max)
			{
				Value = Max;
			}
		}

		public void Serialize(BinaryWriter writer) //im going to buy fortnite season 6 battle pass
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
			}
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
}
