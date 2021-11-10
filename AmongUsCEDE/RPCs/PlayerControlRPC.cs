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
using AmongUsCEDE.Lua;
using AmongUsCEDE.LuaData;

namespace AmongUsCEDE.RPCs
{
	[HarmonyPatch(typeof(PlayerControl))]
	[HarmonyPatch("HandleRpc")]
	class PlayerControlRPC
	{


		static bool Prefix(PlayerControl __instance, byte callId, ref MessageReader reader)
		{
			switch (callId)
			{
				case 2:
					PlayerControl.GameOptions = GameOptionsData.FromBytes(reader.ReadBytesAndSize());
					GameOptionsExtension.Deserialize(reader);
					return false;
				case 3: //figure out what to do with this
					return false;
					bool dointro = reader.ReadBoolean();
					GameFunctions.SetRoles(__instance, reader.ReadBytesAndSize(), reader.ReadBytesAndSize(), dointro); //reads the two arrays for players and roles
				case 12:
					if (GameData.Instance == null) return false;
					PlayerControl user = MessageExtensions.ReadNetObject<PlayerControl>(reader);
					PlayerControl target = MessageExtensions.ReadNetObject<PlayerControl>(reader);
					bool DoAnimation = reader.ReadBoolean();
					if (AmongUsClient.Instance.AmClient)
					{
						if (DoAnimation)
						{
							if (PlayerControl.LocalPlayer == user)
							{
								if (Minigame.Instance)
								{
									try
									{
										Minigame.Instance.Close();
										Minigame.Instance.Close();
									}
									catch
									{
									}
								}
							}
							user.MurderPlayer(target);
						}
						else
						{
							GameFunctions.MurderPlayerNoAnimation(target, user);
						}
					}

					return false;
				case (int)CustomRPCs.CheckWithHost: //with every among us update check to make sure this isn't overwritten
					HostRequestType request = (HostRequestType)reader.ReadByte();
					switch (request)
					{
						case HostRequestType.UsePrimary:
							GameFunctions.SendRequestToHost(__instance, request, reader.ReadByte());
							break;
						case HostRequestType.Custom:
							GameFunctions.SendRequestToHost(__instance, request, (PlayerInfoLua)__instance, reader.ReadByte(), reader.ReadDynValueList().ToArray()); //this is complete
							return false;
						default:
							return false;
					}
					return false;

				case (int)CustomRPCs.SetUserData:
					byte player = reader.ReadByte();
					int data = reader.ReadPackedInt32();
					int toset = reader.ReadPackedInt32();
					GameData.PlayerInfo meplayer = GameData.Instance.GetPlayerById(player);
					meplayer.GetExtension().Userdata[data] = toset;


					return false;
			}

			return true;
		}
	}
}
