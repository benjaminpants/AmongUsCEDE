using System;
using System.Collections.Generic;
using System.Text;

namespace AmongUsCEDE.Core
{
	public static class DebugLog
	{
		public static void ShowMessage(string msg)
		{
			System.Diagnostics.Trace.WriteLine(msg);
		}

		public static void ShowMessage(object obj)
		{
			ShowMessage(obj.ToString());
		}

		public static void PrintArray(object[] array)
		{
			string concat = "(";
			for (int i = 0; i < array.Length; i++)
			{
				concat += array[i].ToString();
			}
			concat += ")";
			ShowMessage(concat);
		}
	}
}
