// AmongUsCEDE.Core.Extensions.PlayerInfoExtensions
using AmongUsCEDE;
using AmongUsCEDE.Core;



namespace AmongUsCEDE.Core.Extensions
{


	public class PlayerInfoExtension
	{
		public byte Role = 0;

		public int[] Userdata = new int[4];

		public bool IsInUse = false;

		public void Flush()
		{
			Role = 0;
			Userdata = new int[4];
			IsInUse = false;
		}
	}


	public static class PlayerInfoExtensions
	{
		public static PlayerInfoExtension[] Extensions = new PlayerInfoExtension[15];

		private static Role NoneRole = new Role("none", "Null");

		public static Role GetRole(this GameData.PlayerInfo me)
		{
			PlayerInfoExtension extend = me.GetExtension();
			if (extend.Role == 0)
			{
				return NoneRole;
			}
			return ScriptManager.CurrentGamemode.Roles[extend.Role - 1];
		}

		public static void FlushAllExtensions()
		{
			for (int i = 0; i < Extensions.Length; i++)
			{
				Extensions[i] = new PlayerInfoExtension();
			}
		}

		public static PlayerInfoExtension GetExtension(this GameData.PlayerInfo me)
		{
			if (!Extensions[me.PlayerId].IsInUse)
			{
				Extensions[me.PlayerId].Flush();
				Extensions[me.PlayerId].IsInUse = true;
			}
			return Extensions[me.PlayerId];
		}

		public static bool AddExtension(this GameData.PlayerInfo me)
		{
			if (Extensions[me.PlayerId].IsInUse)
			{
				return false;
			}
			Extensions[me.PlayerId] = new PlayerInfoExtension();
			return true;
		}
	}

}