using System;
using System.Collections.Generic;
using System.Text;
using MoonSharp.Interpreter;

namespace AmongUsCEDE.LuaReimplments
{
	public class TableReimplement
	{
		public void insert(ref Table table, DynValue value)
		{
			table.Append(value);
		}

		public void insert(ref Table table, int insertat, DynValue value)
		{
			throw new NotImplementedException();
		}
	}
}
