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


	}




	internal class RolePatches
	{

	}
}
