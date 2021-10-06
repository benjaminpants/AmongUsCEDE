// AmongUsCEDE.Core.ImpostorSelectPatch
using System.Collections.Generic;
using System.Linq;
using AmongUsCEDE;
using AmongUsCEDE.Core;
using AmongUsCEDE.LuaData;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MoonSharp.Interpreter;

[HarmonyPatch(typeof(ShipStatus))]
[HarmonyPatch("SelectInfected")]
internal class ImpostorSelectPatch
{
	private static bool Prefix()
	{
		Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> playerlist = GameData.Instance.AllPlayers;
		System.Collections.Generic.List<PlayerInfoLua> listfixed = new System.Collections.Generic.List<PlayerInfoLua>();
		for (int i = 0; i < playerlist.Count; i++)
		{
			listfixed.Add(new PlayerInfoLua(playerlist[i]));
		}
		System.Collections.Generic.List<PlayerInfoLua> list = (from pcd in listfixed
			where !pcd.Disconnected
			select pcd into pc
			where !pc.IsDead
			select pc).ToList();
		DynValue dyn = ScriptManager.RunCurrentGMFunction("SelectRoles", false, new object[1]
		{
			list.ToArray()
		});
		if (dyn.Type != DataType.Table)
		{
			return false;
		}
		if (dyn.Table.Get(1).Type != DataType.Table)
		{
			return false;
		}
		if (dyn.Table.Get(2).Type != DataType.Table)
		{
			return false;
		}
		Table tab = dyn.Table.Get(1).Table;
		Table roletab = dyn.Table.Get(2).Table;
		System.Collections.Generic.List<GameData.PlayerInfo> implist = new System.Collections.Generic.List<GameData.PlayerInfo>();
		for (int k = 1; k < tab.Length + 1; k++)
		{
			DynValue dynin = tab.Get(k);
			if (dynin.Type == DataType.UserData)
			{
				PlayerInfoLua playfolua = (PlayerInfoLua)dynin.UserData.Object;
				implist.Add(playfolua.refplayer);
			}
		}
		System.Collections.Generic.List<string> rolenames = new System.Collections.Generic.List<string>();
		for (int j = 1; j < roletab.Length + 1; j++)
		{
			DynValue dynin2 = roletab.Get(j);
			if (dynin2.Type == DataType.String)
			{
				rolenames.Add(dynin2.String);
			}
		}
		GameFunctions.RpcSetRoles(PlayerControl.LocalPlayer, implist.ToArray(), rolenames.ToArray());
		return false;
	}
}
