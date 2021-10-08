// AmongUsCEDE.LuaData.PlayerInfoLua
using AmongUsCEDE.Core.Extensions;
using MoonSharp.Interpreter;
using UnityEngine;





namespace AmongUsCEDE.LuaData
{
	[MoonSharpUserData]
	public class PlayerInfoLua
	{
		[MoonSharpHidden]
		public GameData.PlayerInfo refplayer;

		[MoonSharpHidden]
		public static GameData.PlayerInfo ShitHolder = new GameData.PlayerInfo(0);

		public byte PlayerId
		{
			get;
			private set;
		}

		public string PlayerName
		{
			get;
			set;
		} = string.Empty;


		public int ColorId
		{
			get;
			set;
		}

		public uint HatId
		{
			get;
			set;
		}

		public uint SkinId
		{
			get;
			set;
		}

		public uint PetId
		{
			get;
			set;
		}

		public bool Disconnected
		{
			get;
			private set;
		}

		public bool IsDead
		{
			get;
			private set;
		}

		public string Role
		{
			get;
			private set;
		}

		public bool IsLocal
		{
			get;
			private set;
		}

		public float PosX
		{
			get;
			private set;
		}

		public float PosY
		{
			get;
			private set;
		}

		public static explicit operator PlayerInfoLua(GameData.PlayerInfo b)
		{
			return new PlayerInfoLua(b);
		}

		public static explicit operator PlayerInfoLua(PlayerControl b)
		{
			return new PlayerInfoLua(b.Data);
		}

		public PlayerInfoLua(GameData.PlayerInfo plf)
		{
			if (plf == null)
			{
				PlayerId = 0;
				PlayerName = "NULL";
				ColorId = 0;
				HatId = 0u;
				SkinId = 0u;
				Disconnected = false;
				IsDead = true;
				Role = "internal";
				PosX = 0f;
				PosY = 0f;
				refplayer = ShitHolder;
				Debug.LogWarning("PL is null! Using Placeholder refplayer... hope shitholder doesn't actually get edited...");
				IsLocal = false;
			}
			else
			{
				PlayerId = plf.PlayerId;
				PlayerName = plf.PlayerName;
				ColorId = plf.ColorId;
				HatId = plf.HatId;
				SkinId = plf.SkinId;
				PetId = plf.PetId;
				Role = plf.GetRole().Internal_Name;
				Disconnected = plf.Disconnected;
				IsDead = plf.IsDead;
				PosX = plf.Object.transform.position.x;
				PosY = plf.Object.transform.position.y;
				IsLocal = plf.PlayerId == PlayerControl.LocalPlayer.PlayerId;
				refplayer = plf;
			}
		}
	}
}
