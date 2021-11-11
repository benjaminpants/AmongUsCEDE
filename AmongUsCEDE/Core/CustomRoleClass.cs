using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;

namespace AmongUsCEDE.Core
{
	class CustomRoleClass : RoleBehaviour
	{
		//NOTES: Make IsImpostor always return false
		public override bool CanUse(IUsable console)
		{
			if (console.TryCast<Vent>())
			{
				return Player.Data.GetRole().CanDo(RoleSpecials.Vent, Player.Data);
			}
			else
			{
				Console actualconsole = console.TryCast<Console>();
				bool havetasks = Player.Data.GetRole().HasTasks;
				return (console != null) && (havetasks || actualconsole.AllowImpostor);
			}
		}


		public override void SpawnTaskHeader(PlayerControl playerControl)
		{
			playerControl.SetImportantText();
		}

		public byte RoleID;

		public CustomRoleClass(byte ID)
		{
			Role = (RoleTypes)999;
			this.TeamType = RoleTeamTypes.Crewmate;
			RoleID = ID;
		}


		public override void Initialize(PlayerControl player)
		{
			//We gotta assume role was defined earlier or we're screwed
			this.Player = player;
			if (!player.AmOwner)
			{
				return;
			}
			Role role = ScriptManager.CurrentGamemode.Roles[RoleID - 1];
			CanUseKillButton = role.CanDo(RoleSpecials.Primary, null);
			if (CanUseKillButton)
			{
				DestroyableSingleton<HudManager>.Instance.KillButton.Show();
				player.SetKillTimer(10f);
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.KillButton.Hide();
			}
			if (role.CanDo(RoleSpecials.Vent, null)) //make it grab the raw value
			{
				DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Hide();
			}
			if (role.CanDo(RoleSpecials.Sabotage, null)) //make it grab the raw value
			{
				DestroyableSingleton<HudManager>.Instance.SabotageButton.Show();
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.SabotageButton.Hide();
			}
			if (role.CanBeSeen(Player.Data, PlayerControl.LocalPlayer.Data))
			{
				player.nameText.color = role.RoleTextColor;
			}
			else
			{
				player.nameText.color = Palette.White;
			}
			this.InitializeAbilityButton();
		}


	}




	internal class RolePatches
	{
	}
}
