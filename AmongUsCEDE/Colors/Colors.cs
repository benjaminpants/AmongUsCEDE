using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace AmongUsCEDE
{

	public static class CustomPalette
	{
		public static List<PlayerColor> PlayerColors = new List<PlayerColor>
		{
			//new PlayerColor(new Color(158f / 255f,1f,251f / 255f),new Color(115f / 255f,194f / 255f,206f / 255f), "Sky")
		};
	}



	public class PlayerColor
	{
		public Color32 Base = Color.white;
		public Color32 Shadow = Color.black;
		public Color32 Visor = Color.blue;
		public bool IsSpecial = false;
		public string Name = "Unnamed Color";
		[JsonIgnore]
		public bool IsFunnyRainbowColor;

		public PlayerColor()
		{

		}

		public PlayerColor(Color Colorr, string CName, bool israinbow = false)
		{
			Base = Colorr;
			Shadow = Colorr * Color.gray;
			Name = CName;
			IsFunnyRainbowColor = israinbow;
			IsSpecial = israinbow;
		}

		public PlayerColor(Color BaseColor, Color ShadowColor, Color VisorColor, bool IsSColor, string CName)
		{
			Base = BaseColor;
			Shadow = ShadowColor;
			Visor = VisorColor;
			Name = CName;
			IsSpecial = IsSColor;
		}
		public PlayerColor(Color BaseColor, Color ShadowColor, string CName)
		{
			Base = BaseColor;
			Shadow = ShadowColor;
			IsSpecial = false;
			Name = CName;
		}

	}
}
