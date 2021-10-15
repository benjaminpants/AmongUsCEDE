using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;

namespace AmongUsCEDE.LuaReimplments
{
	[MoonSharpUserData]
	public class CERandom
	{
		public System.Random rng = new System.Random();

		public DynValue GetRandom()
		{
			return DynValue.NewNumber(rng.NextDouble());
		}

		public double GetRandom(double min, double max)
		{
			bool is_int = (Math.Round(min) == min && Math.Round(max) == max);
			if (is_int)
			{
				return rng.Next((int)min,(int)max);
			}
			else
			{
				return (min + (max - min) * rng.NextDouble());
			}
		}

		public void SetSeed(int seed)
		{
			rng = new System.Random(seed);
		}



	}
}
