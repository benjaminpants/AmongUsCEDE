using System;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core
{
	//This class is a non-language specific version of the DynValue class.
	enum HolderDataType
	{
		Null,
		Bool,
		Number,
		String,
		List,
		Dictionary,
		Table,
		PlayerRef,
		PlayerInfo,
		Object
	}

	class DataHolder
	{

		public HolderDataType Type = HolderDataType.Null;

		private object value = null;

		private List<DataHolder> list;

		private Dictionary<DataHolder, DataHolder> dict;


		public bool CanBeConvertedToList()
		{
			return (Type == HolderDataType.List || Type == HolderDataType.Table);
		}

		public bool Bool
		{
			get
			{
				if (Type == HolderDataType.Bool)
				{
					return (bool)value;
				}
				else
				{
					throw new InvalidCastException();
				}
			}
		}

		public double Number
		{
			get
			{
				if (Type == HolderDataType.Number)
				{
					return (double)value;
				}
				else
				{
					throw new InvalidCastException();
				}
			}
		}

		public string String
		{
			get
			{
				if (Type == HolderDataType.String)
				{
					return (string)value;
				}
				else
				{
					if (value != null)
					{
						return value.ToString();
					}
					else
					{
						throw new InvalidCastException();
					}
				}
			}
		}









	}
}
