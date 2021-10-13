// AmongUsCEDE.Core.Patches.VentPatch
using AmongUsCEDE.Core.Extensions;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(Vent))]
[HarmonyPatch("CanUse")]
internal class VentPatch
{
	private static bool Prefix(Vent __instance, GameData.PlayerInfo pc)
	{
		PlayerControl @object = pc.Object;
		if (!pc.GetRole().CanDo(RoleSpecials.Vent,pc))
		{
			return true;
		}
		Vector3 center = @object.Collider.bounds.center;
		Vector3 position = __instance.transform.position;
		float num = Vector2.Distance(center, position);
		if (num <= __instance.UsableDistance && !PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, useTriggers: false))
		{
			pc.IsImpostor = true;
		}
		return true;
	}

	private static void Postfix(Vent __instance, GameData.PlayerInfo pc, ref bool couldUse)
	{
		couldUse = pc.GetRole().CanDo(RoleSpecials.Vent, pc) && (!pc.IsDead && (pc.Object.CanMove || pc.Object.inVent));
		if (pc.IsImpostor)
		{
			pc.IsImpostor = false;
		}
	}
}
