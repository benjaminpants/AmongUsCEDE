using System;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core
{
	public static class GameOptionsExtension
	{
		public static int Gamemode = 0;

		public static void ReturnToDefault()
		{
			Gamemode = 0;
		}

	}
}
