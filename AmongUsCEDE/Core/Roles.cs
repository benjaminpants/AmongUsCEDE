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
			this.Name = Display;
			Internal_Name = Name;
			AvailableSpecials = new List<RoleSpecials>();
		}

		public override string ToString()
		{
			return Name + ":" + Internal_Name + "\nColor:" + RoleColor.ToString();
		}

		public bool CanBeSeen(GameData.PlayerInfo playfo)
		{
			switch(Visibility)
			{
				case RoleVisibility.None:
					return playfo.PlayerId == PlayerControl.LocalPlayer.PlayerId;
				case RoleVisibility.SameRole:
					return playfo.GetRole().Internal_Name == Internal_Name;
				case RoleVisibility.SameTeam:
					return playfo.GetRole().Team == Team;
				case RoleVisibility.SameLayer:
					return playfo.GetRole().Layer == Layer;
				case RoleVisibility.Script:
					throw new NotImplementedException("RoleVisibility.Script has not been implemented yet!");
				case RoleVisibility.Everyone:
					return true;

				default:
					throw new NotImplementedException("RoleVisibility is set to an invalid/undefined state! (" + Visibility + ")");
			}
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
