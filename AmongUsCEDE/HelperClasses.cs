using System;
using System.Collections.Generic;
using System.Text;



namespace AmongUsCEDE.HelperExtensions
{
	public static class HelperFunctions
	{
		public static List<TSource> ToProperList<TSource>(this Il2CppSystem.Collections.Generic.List<TSource> me)
		{
			List<TSource> list = new List<TSource>();
			for (int i = 0; i < me.Count; i++)
			{
				list.Add(me[i]);
			}
			return list;
		}
	}
}
