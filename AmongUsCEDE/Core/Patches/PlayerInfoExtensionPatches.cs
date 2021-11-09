using System;
using System.Collections.Generic;
using System.Text;
using AmongUsCEDE.Core.Extensions;
using BepInEx;
using HarmonyLib;
using InnerNet;
using Hazel;

namespace AmongUsCEDE.Core.Patches
{

	internal class JoinAndLeavingPatches
	{
		[HarmonyPatch(typeof(GameData))]
		[HarmonyPatch("AddPlayer")]
		class PlayerJoinPatch
		{
			static void Postfix(GameData.PlayerInfo __result)
			{
				__result.AddExtension();

			}
		}

		[HarmonyPatch(typeof(GameData))]
		[HarmonyPatch("RemovePlayer")]
		class PlayerRemovePatch
		{
			static void Postfix(byte playerId, bool __result)
			{
				if (!__result) return;
				PlayerInfoExtensions.Extensions[playerId].IsInUse = false;

			}
		}

		[HarmonyPatch(typeof(GameData))]
		[HarmonyPatch("HandleDisconnect", new System.Type[0])]
		class HandleDisconnectPatch
		{
			static void Postfix()
			{
				if (!AmongUsClient.Instance.IsGameStarted)
				{
					for (int i = GameData.Instance.AllPlayers.Count - 1; i >= 0; i--)
					{
						if (!GameData.Instance.AllPlayers[i].Object)
						{
							PlayerInfoExtensions.Extensions[i].IsInUse = false;
						}
					}
				}

			}
		}

		[HarmonyPatch(typeof(GameData))]
		[HarmonyPatch("HandleDisconnect", typeof(PlayerControl), typeof(DisconnectReasons))]
		class HandleDisconnectTheSecondPatch
		{
			static void Prefix(PlayerControl player, DisconnectReasons reason)
			{

				if (!player)
				{
					return;
				}
				GameData.PlayerInfo playerById = GameData.Instance.GetPlayerById(player.PlayerId);
				if (playerById == null)
				{
					return;
				}

				PlayerInfoExtensions.Extensions[playerById.PlayerId].IsInUse = false;




			}
		}
	
	}


	[HarmonyPatch(typeof(AmongUsClient))]
	[HarmonyPatch("ExitGame")]
	class ExitGamePatch
	{
		static void Prefix()
		{
			PlayerInfoExtensions.FlushAllExtensions();
		}
	}


	internal class PlayerInfoSerializeAndDeserializePatches
	{
		[HarmonyPatch(typeof(GameData.PlayerInfo))]
		[HarmonyPatch("Serialize")]
		class SerializePatch
		{
			static void Postfix(GameData.PlayerInfo __instance, ref MessageWriter writer)
			{
				PlayerInfoExtension ext = __instance.GetExtension();
				writer.Write((byte)ext.Role);
				for (int i = 0; i < AmongUsCEDE.Lua_UserDataAmount; i++)
				{
					writer.WritePacked(ext.Userdata[i]);
				}
				
			}
		}

		[HarmonyPatch(typeof(GameData.PlayerInfo))]
		[HarmonyPatch("Deserialize")]
		class DeserializePatch
		{
			static void Postfix(GameData.PlayerInfo __instance, ref MessageReader reader)
			{
				PlayerInfoExtension ext = __instance.GetExtension();
				ext.Role = reader.ReadByte();
				for (int i = 0; i < AmongUsCEDE.Lua_UserDataAmount; i++)
				{
					ext.Userdata[i] = reader.ReadPackedInt32();
				}
			
			}
		}
	}
}


