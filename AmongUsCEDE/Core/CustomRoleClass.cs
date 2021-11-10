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

		public override void Initialize(PlayerControl player)
		{
			//We gotta assume role was defined earlier or we're screwed
			this.Player = player;
			if (!player.AmOwner)
			{
				return;
			}
			if (this.CanUseKillButton)
			{
				DestroyableSingleton<HudManager>.Instance.KillButton.Show();
				DestroyableSingleton<HudManager>.Instance.SabotageButton.Show();
				DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
				player.SetKillTimer(10f);
			}
			else
			{
				DestroyableSingleton<HudManager>.Instance.KillButton.Hide();
				DestroyableSingleton<HudManager>.Instance.SabotageButton.Hide();
				DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Hide();
			}
			player.nameText.color = this.NameColor;
			this.InitializeAbilityButton();
		}


	}




	internal class RolePatches
	{

	}
}
