using System;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core.CustomSettings
{
	public class Setting
	{
		public string internal_name = "internal_setting_name";
		public string display_name = "SUS SETTING:";

		public object Value;

		public override string ToString()
		{
			return display_name + ":" + Value.ToString();
		}
	}
}
