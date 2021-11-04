using System;
using System.Collections.Generic;
using System.Text;
using Hazel;
using MoonSharp.Interpreter;
using AmongUsCEDE.Core;
using AmongUsCEDE.LuaData;

namespace AmongUsCEDE.Lua
{
	public static class LuaExtensions
	{
		public static void Write(this MessageWriter me, DynValue value)
		{
			me.Write((byte)value.Type);
			switch (value.Type)
			{
				case DataType.Number:
					me.Write((float)value.Number);
					break;
				case DataType.Boolean:
					me.Write(value.Boolean);
					break;
				case DataType.Nil:
					//write nothing, but don't throw an exception
					break;
				case DataType.Table:
					if (value.Table.Length >= 256) throw new CriticalClientException("Writing tables with lengths greater then 255 is not supported!");
					me.Write((byte)value.Table.Length);
					foreach (DynValue dyn in value.Table.Keys)
					{
						me.Write(dyn);
						me.Write(value.Table.Get(dyn));
					}
					break;
				case DataType.String:
					me.Write(value.String);
					break;
				case DataType.UserData:
					if (value.UserData.Descriptor.Type == typeof(PlayerInfoLua))
					{
						me.Write((byte)1);
						me.Write(((PlayerInfoLua)value.UserData.Object).PlayerId);
					}
					else
					{
						me.Write((byte)0);
					}
					break;
				default:
					throw new CriticalClientException(value.Type + " not implemented for serializing");
			}
		}

		public static void Write(this MessageWriter me, List<DynValue> values)
		{
			if (values.Count >= 256) throw new CriticalClientException("Attempting to serialize values greater then 255!");
			me.Write((byte)values.Count);
			for (int i = 0; i < values.Count; i++)
			{
				me.Write(values[i]);
			}
		}

		public static List<DynValue> ReadDynValueList(this MessageReader me)
		{
			List<DynValue> values = new List<DynValue>();
			byte length = me.ReadByte();
			for (int i = 0; i < length; i++)
			{
				values.Add(me.ReadDynValue());
			}
			return values;
		}

		public static DynValue ReadDynValue(this MessageReader me)
		{
			DataType type = (DataType)me.ReadByte();
			switch(type)
			{
				case DataType.Number:
					return DynValue.NewNumber(me.ReadSingle());
				case DataType.String:
					return DynValue.NewString(me.ReadString());
				case DataType.Nil:
					return DynValue.NewNil();
				case DataType.Boolean:
					return DynValue.NewBoolean(me.ReadBoolean());
				case DataType.UserData:
					switch(me.ReadByte())
					{
						case 1:
							if (!GameData.Instance) break;
							return UserData.Create((PlayerInfoLua)GameData.Instance.GetPlayerById(me.ReadByte()));
					}
					return DynValue.NewNil();
				case DataType.Table: //TODO: Figure out if theres a way to create a Table without a Script reference.
					UnityEngine.Debug.Log("Table Reading not implemented!");
					return DynValue.NewNil();
				default:
					return DynValue.NewNil();
			}
		}

	}
}
