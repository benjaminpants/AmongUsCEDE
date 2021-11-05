using System;
using UnityEngine;
using System.Collections.Generic;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;
using MoonSharp.Interpreter;


public enum RoleSpecials
{
	Primary,
	Sabotage,
	Vent,
	Report
}


public enum PrimaryTarget
{
	Others,
	Self,
	SelfIfNone
}

public enum RoleVisibility
{
	None,
	SameRole,
	SameTeam,
	SameLayer,
	Script,
	Everyone
}

namespace AmongUsCEDE.Core
{
	public class Role
	{
		public Role(string Name, string Display)
		{
			this.Name = Display;
			Internal_Name = Name;
			AvailableSpecials = new List<RoleSpecials>();
		}

		public override string ToString()
		{
			return Name + ":" + Internal_Name + "\nColor:" + RoleColor.ToString();
		}

		public bool CanBeSeen(GameData.PlayerInfo holder, GameData.PlayerInfo playfo)
		{
			if (holder.PlayerId == playfo.PlayerId) return true;
			switch (Visibility)
			{
				case RoleVisibility.None:
					return holder.PlayerId == playfo.PlayerId;
				case RoleVisibility.SameRole:
					return playfo.GetRole().Internal_Name == Internal_Name;
				case RoleVisibility.SameTeam:
					return playfo.GetRole().Team == Team;
				case RoleVisibility.SameLayer:
					return playfo.GetRole().Layer == Layer;
				case RoleVisibility.Script:
					return ScriptManager.RunCurrentGMFunction("CanSeeRole",false, Internal_Name, (PlayerInfoLua)holder, (PlayerInfoLua)playfo).Boolean;
				case RoleVisibility.Everyone:
					return true;

				default:
					throw new NotImplementedException("RoleVisibility is set to an invalid/undefined state! (" + Visibility + ")");
			}
		}



		public bool CanDo(RoleSpecials special, GameData.PlayerInfo pc)
		{
			if (Internal_Name == "None") return false;
			if (special == RoleSpecials.Vent && pc != null)
			{
				DynValue val = ScriptManager.CallCurrentGMHooks("CanVent",(PlayerInfoLua)pc, AvailableSpecials.Contains(RoleSpecials.Vent));
				if (val != null)
				{
					if (val.Type == DataType.Boolean)
					{
						return val.Boolean;
					}
				}
			}
			return AvailableSpecials.Contains(special);
		}


		public string Name;

		public string Internal_Name;

		public Color RoleColor = Color.white;

		public Color RoleTextColor = Color.white;

		public bool HasTasks = true;

		public bool ImmuneToAffectors;

		public byte Layer;

		public PrimaryTarget PrimaryTargets;

		public List<RoleSpecials> AvailableSpecials;

		public RoleVisibility Visibility;

		public byte Team;

		public string Reveal_Text;

		public string FakeTaskString = "";

	}
}
