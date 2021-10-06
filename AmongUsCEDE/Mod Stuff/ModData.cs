using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using AmongUsCEDE.Core;

namespace AmongUsCEDE.Mods
{

	public enum ScriptType
	{
		Gamemode,
		Plugin,
		Other
	}
	public class Mod
	{
		[JsonIgnore]
		public string BaseDirectory;
		[JsonProperty("Name")]
		public string ModName;
		[JsonProperty("Description")]
		public string ModDesc;
		[JsonProperty("File Version")]
		public int FileVersion = -1;
		[JsonProperty("Dependencies")]
		public string[] Dependencies = new string[0];
		[JsonIgnore]
		public bool Enabled = true;
		[JsonIgnore]
		public List<Gamemode> Gamemodes = new List<Gamemode>();

		public Mod()
		{
			ModName = "Null";
			ModDesc = "Oopsie! Someone forgot to put a description!";
		}

		public Mod(string title, string desc)
		{
			ModName = title;
			ModDesc = desc;
		}


		public override string ToString()
		{
			return ModName == "" ? "Unnamed Mod" : ModName;
		}

		public Mod(string bd, string mn, string md, bool en = true)
		{
			BaseDirectory = bd;
			ModName = mn;
			ModDesc = md;
			Enabled = en;
		}



	}
}
