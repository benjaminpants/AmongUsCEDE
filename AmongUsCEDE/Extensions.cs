using UnityEngine;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace AmongUsCEDE.Extensions
{
	public static class CEExtensions
	{

		public static string GetGameDirectory()
		{
			return System.IO.Directory.GetParent(Application.dataPath).FullName;
		}

		public static string GetTexturesDirectory(string ExtraDir = null)
		{
			if (string.IsNullOrEmpty(ExtraDir)) return System.IO.Path.Combine(Application.dataPath, "CE_Assets", "Textures");
			else return System.IO.Path.Combine(Application.dataPath, "CE_Assets", "Textures", ExtraDir);
		}


		private static bool IsCharacterAVowel(char c)
		{
			string vowels = "aeiou";
			return vowels.IndexOf(c.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0;
		}


		public static string AOrAn(this string input, bool firstlettercap)
		{
			return (firstlettercap ? "A" : "a") + (IsCharacterAVowel(input[0]) ? "n" : ""); //thanks rose, developer on polus.gg!!!!


		}

		public static string ToHtmlStringRGBA(this Color32 color)
		{
			return $"{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
		}

		public static string ToHtmlStringRGBA(this Color color)
		{
			return ((Color32)color).ToHtmlStringRGBA();
		}

	}
}