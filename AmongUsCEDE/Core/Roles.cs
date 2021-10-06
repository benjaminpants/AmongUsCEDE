using System;
using UnityEngine;
using System.Collections.Generic;
using AmongUsCEDE.Core.Extensions;
using AmongUsCEDE.LuaData;


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
			RoleName = Display;
			UUID = Name;
			AvailableSpecials = new List<RoleSpecials>();
		}

		public override string ToString()
		{
			return RoleName + ":" + UUID + "\nColor:" + RoleColor.ToString();
		}

		public bool CanBeSeen(GameData.PlayerInfo playfo)
		{
			switch(RoleVisibility)
			{
				case RoleVisibility.None:
					return playfo.PlayerId == PlayerControl.LocalPlayer.PlayerId;
				case RoleVisibility.SameRole:
					return playfo.GetRole().UUID == UUID;
				case RoleVisibility.SameTeam:
					return playfo.GetRole().RoleTeam == RoleTeam;
				case RoleVisibility.SameLayer:
					return playfo.GetRole().Layer == Layer;
				case RoleVisibility.Script:
					throw new NotImplementedException("RoleVisibility.Script has not been implemented yet!");
				case RoleVisibility.Everyone:
					return true;

				default:
					throw new NotImplementedException("RoleVisibility is set to an invalid/undefined state! (" + RoleVisibility + ")");
			}
		}


		public string RoleName;

		public string UUID;

		public Color RoleColor = Color.white;

		public Color RoleTextColor = Color.white;

		public bool HasTasks;

		public byte Layer;

		public PrimaryTarget PrimaryTargets;

		public List<RoleSpecials> AvailableSpecials;

		public RoleVisibility RoleVisibility;

		public byte RoleTeam;

		public string RoleText;

		public string FakeTaskString = "";

		public bool UseImpVision;

	}
}
