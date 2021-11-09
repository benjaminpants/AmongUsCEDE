// AmongUsCEDE.LuaData.PlayerInfoLua
using AmongUsCEDE.Core.Extensions;
using MoonSharp.Interpreter;
using UnityEngine;
using AmongUsCEDE.Core;





namespace AmongUsCEDE.LuaData
{
	[MoonSharpUserData]
	public class PlayerInfoLua
	{
		[MoonSharpHidden]
		public GameData.PlayerInfo refplayer;

		public byte ID
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			set;
		} = string.Empty;


		public int Color
		{
			get;
			set;
		}

		public uint Hat
		{
			get;
			set;
		}

		public uint Skin
		{
			get;
			set;
		}

		public uint Pet
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

		public bool InVent
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


		public byte Team
		{
			get;
			private set;
		}


		public byte Layer
		{
			get;
			private set;
		}

		public int[] UserData
		{
			get;
			private set;
		}

		public void SetUserDataValue(int value, int toset) //this isn't tested
		{
			if (!AmongUsClient.Instance.AmHost) return;
			SetFunctions.RPCSetUserData(PlayerControl.LocalPlayer, refplayer.Object, value - 1, toset);
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
				throw new System.NullReferenceException();
			}
			else
			{
				ID = plf.PlayerId;
				Name = plf.PlayerName;
				Color = plf.ColorId;
				Hat = plf.HatId;
				Skin = plf.SkinId;
				Pet = plf.PetId;
				Role = plf.GetRole().Internal_Name;
				Layer = plf.GetRole().Layer;
				Team = plf.GetRole().Team;
				Disconnected = plf.Disconnected;
				IsDead = plf.IsDead;
				PosX = plf.Object.transform.position.x;
				PosY = plf.Object.transform.position.y;
				IsLocal = plf.PlayerId == PlayerControl.LocalPlayer.PlayerId;
				refplayer = plf;
				InVent = refplayer.Object.inVent;
				UserData = refplayer.GetExtension().Userdata;
			}
		}
	}
}
