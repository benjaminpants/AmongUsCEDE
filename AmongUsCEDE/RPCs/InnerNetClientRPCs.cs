using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;
using BepInEx;
using HarmonyLib;
using InnerNet;
using Hazel;
using PowerTools;
using UnityEngine;
using AmongUsCEDE.Core;

namespace AmongUsCEDE.RPCs
{
	[HarmonyPatch(typeof(InnerNetClient))]
	[HarmonyPatch("HandleMessage")]
	class InnerNetClientRPC
	{
		static bool Prefix(InnerNetClient __instance,ref MessageReader reader, SendOption sendOption)
		{
			switch (reader.Tag)
			{
				case 8:
					{
						int num3 = reader.ReadInt32();
						if (__instance.GameId == num3 && __instance.GameState != InnerNetClient.GameStates.Ended)
						{
							__instance.GameState = InnerNetClient.GameStates.Ended;
							lock (__instance.allClients)
							{
								__instance.allClients.Clear();
							}
							VictoryTypes victype = (VictoryTypes)reader.ReadByte();
							byte amount = reader.ReadByte();
							byte[] playerids = new byte[amount];
							for (int i = 0; i < amount; i++)
							{
								playerids[i] = reader.ReadByte();
							}
							string song = reader.ReadString();

							GameFunctions.CommenceVictory(AmongUsClient.Instance,playerids,song,victype);
							
							//goto IL_263;
						}
						return false;
					}
			}
			return true;
		}
	}
}
